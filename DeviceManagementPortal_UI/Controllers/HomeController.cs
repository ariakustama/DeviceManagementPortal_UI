using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeviceManagementPortal_UI.Models;
using Microsoft.AspNetCore.Authorization;
using DataTables.AspNet.Core;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using DeviceManagementPortal_UI.Extensions;
using DataTables.AspNet.AspNetCore;

namespace DeviceManagementPortal_UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApiConfig _apiConfig;
        ApiAliasConfig _apiAliasConfig;
        private HttpClient _client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IOptions<ApiConfig> apiConfig,
            IOptions<ApiAliasConfig> apiAliasConfig)
        {
            _logger = logger;
            _apiConfig = apiConfig.Value;
            _apiAliasConfig = apiAliasConfig.Value;
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> DataTableDevices(IDataTablesRequest request, string param) 
        {
            int oPage = 1;
            int returnPageNumber = 0;
            int returnTotalRecords = 0;

            try
            {
                if (request.Start > 0)
                {
                    oPage = (request.Start / 10) + 1;
                }

                var contentType = "application/json";
                var dataParamToApi = new ParamGetListDevices();

                dataParamToApi.page = oPage;
                dataParamToApi.itemPerPage = 10;

                ApiResult<ReturnGetListDevicesViewModel> resutlApi = new ApiResult<ReturnGetListDevicesViewModel>();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var dataParameter = JsonConvert.SerializeObject(dataParamToApi);
                StringContent contentData = new StringContent(dataParameter, Encoding.UTF8, "application/json");

                resutlApi = await _client.PostAsync<ReturnGetListDevicesViewModel>($"{_apiConfig.ApiUrl}{_apiAliasConfig.GetDevicesPerPagging}", contentData);
                if (!resutlApi.isSuccessful)
                {
                    throw new ArgumentException(resutlApi.message);
                }

                var datafromApi = resutlApi.Payload.listDevices.AsQueryable();

                if (datafromApi.Any())
                {
                    returnPageNumber = oPage;
                    returnTotalRecords = resutlApi.Payload.CountData;
                }

                #region Shorted
                IOrderedQueryable<DevicesViewModel> dataSorted = null;
                #endregion
                var response = DataTablesResponse.Create(request, returnPageNumber, returnTotalRecords, dataSorted != null ? dataSorted : datafromApi);

                return new DataTablesJsonResult(response, true);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public IActionResult DevicesCreate()
        {

            return View();
        }

        public async Task<IActionResult> DevicesDetail(string id)
        {
            ApiResult<DevicesViewModel> resutlApi = new ApiResult<DevicesViewModel>();
            resutlApi = await _client.GetAsync<DevicesViewModel>($"{_apiConfig.ApiUrl}{_apiAliasConfig.GetDeviceById}{id}");
            if (!resutlApi.isSuccessful)
            {
                return View("/Home/Error");
            }
            ViewBag.Mode = "Detail";
            return View("DevicesEdit", resutlApi.Payload);
        }

        public async Task<IActionResult> DevicesEdit(string id)
        {
            ApiResult<DevicesViewModel> resutlApi = new ApiResult<DevicesViewModel>();
            resutlApi = await _client.GetAsync<DevicesViewModel>($"{_apiConfig.ApiUrl}{_apiAliasConfig.GetDeviceById}{id}");
            if (!resutlApi.isSuccessful)
            {
                return View("/Home/Error");
            }
            ViewBag.Mode = "Edit";
            return View(resutlApi.Payload);
        }

        [HttpPost]
        public async Task<JsonResult> GetListBackEndMapping(string id) 
        {
            try
            {
                ApiResult<List<BackEndViewModel>> resutlApi = new ApiResult<List<BackEndViewModel>>();
                resutlApi = await _client.GetAsync<List<BackEndViewModel>>($"{_apiConfig.ApiUrl}{_apiAliasConfig.GetListBackEndMapping}{id}");
                if (!resutlApi.isSuccessful)
                {
                    throw new ArgumentException(resutlApi.message);
                }
                return Json(new { isError = false, data = resutlApi.Payload });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DevicesSubmit([FromForm]DevicesSubmitViewModel param)
        {
            try
            {
                var contentType = "application/json";
                ApiResult<bool> resutlApi = new ApiResult<bool>();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var dataParameter = JsonConvert.SerializeObject(param);
                StringContent contentData = new StringContent(dataParameter, Encoding.UTF8, "application/json");

                if (param.Mode == "Create")
                {
                    resutlApi = await _client.PostAsync<bool>($"{_apiConfig.ApiUrl}{_apiAliasConfig.SaveDevicesAndBackEndMapping}", contentData);
                    if (!resutlApi.isSuccessful)
                    {
                        throw new ArgumentException(resutlApi.message);
                    }
                }
                else
                {
                    resutlApi = await _client.PostAsync<bool>($"{_apiConfig.ApiUrl}{_apiAliasConfig.UpdateDevicesAndBackEndMapping}", contentData);
                    if (!resutlApi.isSuccessful)
                    {
                        throw new ArgumentException(resutlApi.message);
                    }
                }
                
                return Json(new { isError = false });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDeviceData(string id)
        {
            try
            {
                ApiResult<bool> resutlApi = new ApiResult<bool>();
                resutlApi = await _client.DeleteAsync<bool>($"{_apiConfig.ApiUrl}{_apiAliasConfig.DeleteDeviceData}{id}");
                if (!resutlApi.isSuccessful)
                {
                    throw new ArgumentException(resutlApi.message);
                }
                return Json(new { isError = false });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message });
            }
        }
    }
}
