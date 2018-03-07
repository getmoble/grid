using System;

namespace Grid.Clients.ITSync.Models
{
    [Serializable]
    public class SoftwareModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string DisplayVersion { get; set; }
    }
}
