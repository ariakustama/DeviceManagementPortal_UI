

namespace DeviceManagementPortal_UI.Models
{
    public class ApiViewModels
    {

    }

    public class ApiConfig
    {
        public string ApiUrl { get; set; }
    }
    public class ApiAliasConfig
    {
        public string GetDevicesPerPagging { get; set; }
        public string SaveDevicesAndBackEndMapping { get; set; }
        public string GetDeviceById { get; set; }
        public string GetListBackEndMapping { get; set; }
        public string UpdateDevicesAndBackEndMapping { get; set; }
        public string DeleteDeviceData { get; set; }
    }
    
}
