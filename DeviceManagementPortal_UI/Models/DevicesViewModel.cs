using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementPortal_UI.Models
{
    public class DevicesViewModel
    {
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string SimCardNo { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormat { get; set; }
        public string CreatedBy { get; set; }
    }

    public class BackEndViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class ReturnGetListDevicesViewModel
    {
        public List<DevicesViewModel> listDevices { get; set; }
        public int CountData { get; set; }
    }

    public class ParamGetListDevices
    {
        public int page { set; get; }
        public int itemPerPage { set; get; }
    }
}
