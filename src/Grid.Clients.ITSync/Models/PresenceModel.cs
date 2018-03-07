using System;

namespace Grid.Clients.ITSync.Models
{
    public class PresenceModel
    {
        public int Source { get; set; }
        public string Payload { get; set; }
        public DateTime RanOn { get; set; }
        public string AssetId { get; set; }
    }
}
