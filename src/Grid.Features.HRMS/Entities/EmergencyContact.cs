using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.Entities
{
    public class EmergencyContact : EntityBase
    {
        public string Name { get; set; }

        public Relationship Relationship { get; set; }

        [UIHint("PhoneNumber")]
        public string Phone { get; set; }

        [UIHint("PhoneNumber")]
        public string Mobile { get; set; }

        [DisplayName("Work Phone")]
        [UIHint("PhoneNumber")]
        public string WorkPhone { get; set; }

        public string Address { get; set; }

        [UIHint("Email")]
        public string Email { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
