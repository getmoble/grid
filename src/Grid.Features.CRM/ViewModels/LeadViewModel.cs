using System;
using Grid.Features.Common;

namespace Grid.Features.CRM.ViewModels
{
    public class LeadViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public DateTime? RecievedOn { get; set; }
        public  string  Status { get; set; }
    }
}