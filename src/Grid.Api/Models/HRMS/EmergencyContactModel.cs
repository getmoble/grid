using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Api.Models.HRMS
{
    public class EmergencyContactModel : ApiModelBase
    {
        public string Name { get; set; }
        public Relationship Relationship { get; set; }
        public string RelationshipType { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Employee { get; set; }
        public int? EmployeeId { get; set; }
  
        public EmergencyContactModel()
        {

        }
        public EmergencyContactModel(EmergencyContact emergencyContact)
        {
            Id = emergencyContact.Id;
            Name = emergencyContact.Name;
            RelationshipType = GetEnumDescription(emergencyContact.Relationship);
            Relationship = emergencyContact.Relationship;
            Phone = emergencyContact.Phone;
            Mobile = emergencyContact.Mobile;
            WorkPhone = emergencyContact.WorkPhone;
            Email = emergencyContact.Email;
            Address = emergencyContact.Address;         
            EmployeeId = emergencyContact.EmployeeId;       
        }
    }
}