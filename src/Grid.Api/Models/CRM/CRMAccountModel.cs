using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Entities.Enums;
using Grid.Features.HRMS.Entities;
using System;

namespace Grid.Api.Models.CRM
{
    public class CRMAccountModel : ApiModelBase
    {
        public string Title { get; set; }
        public string Industry { get; set; }
        public EmployeeCount EmployeeCount { get; set; }
        public DateTime? FoundedOn { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string SecondaryEmail { get; set; }
        public string OfficePhone { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string LinkedIn { get; set; }
        public string City { get; set; }
        public string Country { get; set; }       
        public string Address { get; set; }
        public string CommunicationAddress { get; set; }     
        public string Expertise { get; set; }
        public string Description { get; set; }
        public int? AssignedToEmployeeId { get; set; }
        public string AssignedToEmployee { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public CRMAccountModel()
        {

        }
        public CRMAccountModel(CRMAccount contact)
        {
            Id = contact.Id;
            if (contact.Parent != null)
            {
                Parent = contact.Parent.Title;
            }

            if (contact.AssignedToEmployee != null)
            {
                AssignedToEmployee = contact.AssignedToEmployee.User.Person.Name;
            }
            ParentId = contact.ParentId;
            AssignedToEmployeeId = contact.AssignedToEmployeeId;
            PhoneNo = contact.PhoneNo;
            Email = contact.Email;
            CreatedOn = contact.CreatedOn;
            Title = contact.Title;
            Industry = contact.Industry;
            EmployeeCount = contact.EmployeeCount;
            FoundedOn = contact.FoundedOn;
            Email = contact.Email;
            SecondaryEmail = contact.SecondaryEmail;
            OfficePhone = contact.OfficePhone;
            Website = contact.Website;
            Facebook = contact.Facebook;
            LinkedIn = contact.LinkedIn;
            Twitter = contact.Twitter;
            Country = contact.Country;
            GooglePlus = contact.GooglePlus;
            City = contact.City;
            Description = contact.Description;
            Country = contact.Country;
            Address = contact.Address;
            CommunicationAddress = contact.CommunicationAddress;
            Expertise = contact.Expertise;

        }
    }
}
