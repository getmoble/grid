using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Entities.Enums;

namespace Grid.Features.CRM.ViewModels
{
    public class AccountDetailsViewModel: ViewModelBase
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Industry")]
        public string Industry { get; set; }

        [DisplayName("Employee Count")]
        public EmployeeCount EmployeeCount { get; set; }

        [DisplayName("Founded On")]
        public DateTime? FoundedOn { get; set; }

        [UIHint("Email")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [UIHint("PhoneNumber")]
        public string PhoneNo { get; set; }

        [DisplayName("Secondary Email")]
        [UIHint("Email")]
        public string SecondaryEmail { get; set; }

        [DisplayName("Office Phone")]
        [UIHint("PhoneNumber")]
        public string OfficePhone { get; set; }

        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        [DisplayName("Google Plus")]
        public string GooglePlus { get; set; }

        [DisplayName("Linked In")]
        public string LinkedIn { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Communication Address")]
        public string CommunicationAddress { get; set; }

        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        public int? AssignedToUserId { get; set; }
        public Features.HRMS.Entities.Employee AssignedToUser { get; set; }

        public int? ParentId { get; set; }
        public CRMAccount Parent { get; set; }

        public List<CRMContact> Contacts { get; set; }

        public AccountDetailsViewModel()
        {
            Contacts = new List<CRMContact>();
        }

        public AccountDetailsViewModel(CRMAccount account): this()
        {
            Id = account.Id;
            Title = account.Title;
            Industry = account.Industry;
            EmployeeCount = account.EmployeeCount;
            FoundedOn = account.FoundedOn;
            Email = account.Email;
            PhoneNo = account.PhoneNo;
            SecondaryEmail = account.SecondaryEmail;
            OfficePhone = account.OfficePhone;
            Website = account.Website;
            Facebook = account.Facebook;
            Twitter = account.Twitter;
            GooglePlus = account.GooglePlus;
            LinkedIn = account.LinkedIn;
            City = account.City;
            Country = account.Country;
            Address = account.Address;
            CommunicationAddress = account.CommunicationAddress;
            Expertise = account.Expertise;
            Description = account.Description;
            AssignedToUserId = account.AssignedToEmployeeId;
            AssignedToUser = account.AssignedToEmployee;
            ParentId = account.ParentId;
            Parent = account.Parent;
        }
    }
}