using DeviceManagementPortal_UI.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementPortal_UI.Extensions
{
    public static class HttpClientExtensions
    {

        private static string ApiErrorCode = "API_ERROR_CODE";

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("ERROR: " + e.ToString());
            }

            return response;
        }

        public static async Task<ApiResult<T>> GetAsync<T>(this HttpClient client, string Url)
        {
            var result = new ApiResult<T>();
            var res = await client.GetAsync(Url);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
            }
            else
            {
                result.isSuccessful = false;
                result.Code = ApiErrorCode;
                result.message = "Failed To Call API";
            }

            return result;
        }

        public static async Task<ApiResult<T>> PostAsync<T>(this HttpClient client, string Url, HttpContent content)
        {
            var result = new ApiResult<T>();
            var res = await client.PostAsync(Url, content);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
            }
            else
            {
                result.isSuccessful = false;
                result.Code = ApiErrorCode;
                result.message = "Failed To Call API";
            }

            return result;
        }

        public static async Task<ApiResult<T>> PostAsync<T>(this HttpClient client, string Url, object content)
        {
            var result = new ApiResult<T>();
            var jsonObj = JsonConvert.SerializeObject(content);
            var jsonContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            return await PostAsync<T>(client, Url, jsonContent);
        }

        public static async Task<ApiResult<T>> PutAsync<T>(this HttpClient client, string Url, HttpContent content)
        {
            var result = new ApiResult<T>();
            var res = await client.PutAsync(Url, content);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
            }
            else
            {
                result.isSuccessful = false;
                result.Code = ApiErrorCode;
                result.message = "Failed To Call API";
            }

            return result;
        }

        public static async Task<ApiResult<T>> PutAsync<T>(this HttpClient client, string Url, object content)
        {
            var result = new ApiResult<T>();
            var jsonObj = JsonConvert.SerializeObject(content);
            var jsonContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            return await PutAsync<T>(client, Url, jsonContent);
        }

        public static async Task<ApiResult<T>> PatchAsync<T>(this HttpClient client, string Url, HttpContent content)
        {
            var result = new ApiResult<T>();
            var res = await client.PatchAsync(new Uri(Url), content);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
            }
            else
            {
                result.isSuccessful = false;
                result.Code = ApiErrorCode;
                result.message = "Failed To Call API";
            }

            return result;
        }

        public static async Task<ApiResult<T>> PatchAsync<T>(this HttpClient client, string Url, object content)
        {
            var result = new ApiResult<T>();
            var jsonObj = JsonConvert.SerializeObject(content);
            var jsonContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            return await PatchAsync<T>(client, Url, jsonContent);
        }

        public static async Task<ApiResult<T>> DeleteAsync<T>(this HttpClient client, string Url)
        {
            var result = new ApiResult<T>();
            var res = await client.DeleteAsync(Url);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
            }
            else
            {
                result.isSuccessful = false;
                result.Code = ApiErrorCode;
                result.message = "Failed To Call API";
            }

            return result;
        }
    }
}
