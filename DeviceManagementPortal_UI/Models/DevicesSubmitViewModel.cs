using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementPortal_UI.Models
{
    public class DevicesSubmitViewModel
    {
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string SimCardNo { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string Mode { get; set; }
        public List<BackEndViewModel> listBackEnd { get; set; }
    }
}
