using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.CRM.Entities.Enums;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMAccount : UserCreatedEntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Industry")]
        public string Industry { get; set; }

        [DisplayName("Employee Count")]
        public EmployeeCount EmployeeCount { get; set; }

        [DisplayName("Founded On")]
        [UIHint("Date")]
        public DateTime? FoundedOn { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(254)]
        [UIHint("Email")]
        public string Email { get; set; }

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

        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        public int? AssignedToEmployeeId { get; set; }
        [ForeignKey("AssignedToEmployeeId")]
        public Employee AssignedToEmployee { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual CRMAccount Parent { get; set; }
    }
}