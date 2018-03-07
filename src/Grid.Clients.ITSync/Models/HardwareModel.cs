using System;

namespace Grid.Clients.ITSync.Models
{
    [Serializable]
    public class HardwareModel
    {
        public string UserDomainName { get; set; }
        public string Username { get; set; }
        public double RamMemory { get; set; }
        public string SystemOS { get; set; }
        public string Processor { get; set; }
        public string Language { get; set; }
        public double HardDiskSize { get; set; }
        public string SystemDirectory { get; set; }
        public int ProcessorCount { get; set; }
        public int SystemPageSize { get; set; }
    }
}
