using System;
using System.Collections.Generic;
using System.ComponentModel;
using Grid.Data.MultiTenancy.Entities;

namespace Grid.TenantManagement.ViewModels
{
    public class TenantDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [DisplayName("Contact Person Name")]
        public string ContactPersonName { get; set; }

        [DisplayName("Contact Person Number")]
        public string ContactPersonNumber { get; set; }

        [DisplayName("Connection String")]
        public string ConnectionString { get; set; }

        [DisplayName("Domain Name")]
        public string DomainName { get; set; }

        [DisplayName("Sub Domain")]
        public string SubDomain { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<TenantScheduledJob> ScheduledJobs { get; set; }

        public TenantDetailsViewModel()
        {
            ScheduledJobs = new List<TenantScheduledJob>();
        }

        public TenantDetailsViewModel(Tenant tenant)
        {
            ScheduledJobs = new List<TenantScheduledJob>();

            Id = tenant.Id;
            Name = tenant.Name;
            Address = tenant.Address;
            Email = tenant.Email;
            ContactPersonName = tenant.ContactPersonName;
            ContactPersonNumber = tenant.ContactPersonNumber;
            ConnectionString = tenant.ConnectionString;
            DomainName = tenant.DomainName;
            SubDomain = tenant.SubDomain;
            Status = tenant.Status;
            CreatedOn = tenant.CreatedOn;
        }
    }
}