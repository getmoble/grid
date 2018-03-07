using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities.Enums;
using System;

namespace Grid.Api.Models.CRM
{
    public class CRMContactModel : ApiModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public int? ParentAccountId { get; set; }
        public string ParentAccount { get; set; }
        public int PersonId { get; set; }
        public string  Person { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string SecondaryEmail { get; set; }
        public string OfficePhone { get; set; }
        public string Website { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string LinkedIn { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string CommunicationAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Expertise { get; set; }
        public string Comments { get; set; }
        
        public CRMContactModel()
        {

        }
        public CRMContactModel(CRMContact contact)
        {
            Id = contact.Id;

            if (contact.ParentAccount != null)
            {
                ParentAccount = contact.ParentAccount.Title;
            }
         
            if (contact.Person != null)
            {
                Name = contact.Person.Name;
            }
            
            PhoneNo = contact.Person.PhoneNo;
            Email = contact.Person.Email;
            CreatedOn = contact.CreatedOn;
            Organization = contact.Person.Organization;
            Gender = contact.Person.Gender;
            ParentAccountId = contact.ParentAccountId;
            PersonId = contact.PersonId;
            Designation = contact.Person.Designation;
            SecondaryEmail = contact.Person.SecondaryEmail;
            OfficePhone = contact.Person.OfficePhone;
            Website = contact.Person.Website;
            Skype = contact.Person.Skype;
            Facebook = contact.Person.Facebook;
            //Status = GetEnumDescription(employee.EmployeeStatus);
            LinkedIn = contact.Person.LinkedIn;
            Twitter = contact.Person.Twitter;
            Country = contact.Person.Country;
            GooglePlus = contact.Person.GooglePlus;
            City = contact.Person.City;
            FirstName = contact.Person.FirstName;
            LastName = contact.Person.LastName;
            Address = contact.Person.Address;
            CommunicationAddress = contact.Person.CommunicationAddress;
            DateOfBirth = contact.Person.DateOfBirth;
            Comments = contact.Comments;
            Expertise = contact.Expertise;

        }
    }
}
