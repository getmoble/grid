using System;

namespace Grid.Clients.ITSync.Models
{
    public class HeartBeatModel
    {
        public int Source { get; set; }
        public string Payload { get; set; }
        public DateTime RanOn { get; set; }
    }
}
