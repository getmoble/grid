using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.ViewModels.User
{
    public class NewUserViewModel: ViewModelBase
    {
        [DisplayName("Employee Code")]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        [Required]
        public string EmployeeCode { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(254)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int AccessRuleId { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int? DepartmentId { get; set; }

        public int? LocationId { get; set; }

        [DisplayName("Seat No")]
        public string SeatNo { get; set; }

        public int? DesignationId { get; set; }

        public int? ShiftId { get; set; }

        public double Salary { get; set; }

        public string Bank { get; set; }

        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [DisplayName("PAN Card")]
        public string PANCard { get; set; }

        [DisplayName("Payment Mode")]
        public PaymentMode PaymentMode { get; set; }

        public int? SalaryId { get; set; }

        public int? ReportingPersonId { get; set; }
        public int? ManagerId { get; set; }

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

        public int[] RoleIds { get; set; }
        public int[] TechnologyIds { get; set; }
        public int[] SkillIds { get; set; }
        public int[] CertificationIds { get; set; }
        public int[] HobbiesId { get; set; }

        public NewUserViewModel()
        {
            PaymentMode = PaymentMode.AccountTransfer;
        }
    }
}