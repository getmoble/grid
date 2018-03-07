using System;
using System.Collections.Generic;

namespace Grid.Clients.ITSync.Models
{
    public class SystemInfo
    {
        public string AssetId { get; set; }

        public List<SoftwareModel> Softwares { get; set; }
        public HardwareModel Hardware { get; set; }
        public DateTime RanOn { get; set; }
        public SystemInfo()
        {
            Softwares = new List<SoftwareModel>();
        }
    }
}
