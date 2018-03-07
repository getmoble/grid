using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class Person: EntityBase
    {
        [DisplayName("First Name")]
        [Column(TypeName = "varchar")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        [Column(TypeName = "varchar")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; }

        [NotMapped]
        public string Name => $"{FirstName} {MiddleName} {LastName}";

        public Gender? Gender { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(254)]
        [UIHint("Email")]
        public string Email { get; set; }

        public string Organization { get; set; }

        public string Designation { get; set; }

        [DisplayName("Phone Number")]
        [Column(TypeName = "varchar")]
        [UIHint("PhoneNumber")]
        public string PhoneNo { get; set; }

        [DisplayName("Secondary Email")]
        [UIHint("Email")]
        public string SecondaryEmail { get; set; }

        [DisplayName("Office Phone")]
        [UIHint("PhoneNumber")]
        public string OfficePhone { get; set; }

        public string Website { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        
        [DisplayName("Google Plus")]
        public string GooglePlus { get; set; }
        
        [DisplayName("Linked In")]
        public string LinkedIn { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Communication Address")]
        public string CommunicationAddress { get; set; }

        [DisplayName("Passport No")]
        public string PassportNo { get; set; }

        [DisplayName("Date of Birth")]
        [UIHint("Date")]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName("Blood Group")]
        public BloodGroup? BloodGroup { get; set; }

        [DisplayName("Marital Status")]
        public MaritalStatus? MaritalStatus { get; set; }

        [DisplayName("Marriage Anniversary")]
        [UIHint("Date")]
        public DateTime? MarriageAnniversary { get; set; }

        public string PhotoPath { get; set; }

        public static Person CreateNewPerson()
        {
            return new Person();
        }
    }
}
