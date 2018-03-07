using System;

namespace Grid.TenantManagement.ViewModels
{
    public class TenantScheduledJobViewModel
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; }

        public string CronExpression { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}