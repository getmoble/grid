using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Features.HRMS.Entities
{
    public class Employee : EntityBase 
    {
        [DisplayName("Employee Code")]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        [Required]
        public string EmployeeCode { get; set; }      

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [DisplayName("Seat No")]
        public string SeatNo { get; set; }

        public int? DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }

        public int? ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        public double Salary { get; set; }

        public string Bank { get; set; }

        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [DisplayName("PAN Card")]
        public string PANCard { get; set; }

        [DisplayName("Payment Mode")]
        public PaymentMode PaymentMode { get; set; }

        public int? ReportingPersonId { get; set; }
        [ForeignKey("ReportingPersonId")]
        public Employee ReportingPerson { get; set; }

        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public Employee Manager { get; set; }

        public float? Experience { get; set; }

        [DisplayName("Date of Joining")]
        [UIHint("Date")]
        public DateTime? DateOfJoin { get; set; }

        [DisplayName("Date of Confirmation")]
        [UIHint("Date")]
        public DateTime? ConfirmationDate { get; set; }

        [DisplayName("Date of Resignation")]
        [UIHint("Date")]
        public DateTime? DateOfResignation { get; set; }

        [DisplayName("Last Date")]
        [UIHint("Date")]
        public DateTime? LastDate { get; set; }

        [DisplayName("Official Email")]
        [UIHint("Email")]
        public string OfficialEmail { get; set; }

        [DisplayName("Official Phone")]
        [UIHint("PhoneNumber")]
        public string OfficialPhone { get; set; }

        [DisplayName("Slack")]
        public string OfficialMessengerId { get; set; }

        [DisplayName("Employee Status")]
        public EmployeeStatus? EmployeeStatus { get; set; }

        [DisplayName("Requires TimeSheet as Mandatory")]
        public bool RequiresTimeSheet { get; set; }


        [InverseProperty("ReportingPerson")]
        public ICollection<Employee> Reportees { get; set; }

        [InverseProperty("Manager")]
        public ICollection<Employee> TeamMembers { get; set; }
    }
}
