using Grid.Clients.ITSync.Models;
using Grid.Entities.Auth;
using Grid.Entities.Company;
using Grid.Entities.HRMS;
using Grid.Entities.HRMS.Enums;
using Grid.Entities.IMS;
using Grid.Entities.LMS.Enums;
using Grid.Entities.PMS;
using Grid.Features.Common;
using Grid.Features.LMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Features.User.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        [DisplayName("Employee Code")]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string EmployeeCode { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(254)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public long AccessRuleId { get; set; }
        public AccessRule AccessRule { get; set; }

        public long PersonId { get; set; }
        public Person Person { get; set; }

        public long? DepartmentId { get; set; }
        public Department Department { get; set; }

        public long? LocationId { get; set; }
        public Location Location { get; set; }

        public string SeatNo { get; set; }

        public long? DesignationId { get; set; }
        public Designation Designation { get; set; }

        public long? ShiftId { get; set; }
        public Shift Shift { get; set; }

        public long? ReportingPersonId { get; set; }
        public Entities.User ReportingPerson { get; set; }

        public long? ManagerId { get; set; }
        public Entities.User Manager { get; set; }

        [DisplayName("Joining Experience")]
        public float? Experience { get; set; }

        [DisplayName("Date Of Join")]
        public DateTime? DateOfJoin { get; set; }

        [DisplayName("Confirmation Date")]
        public DateTime? ConfirmationDate { get; set; }

        [DisplayName("Date of Resignation")]
        public DateTime? DateOfResignation { get; set; }

        [DisplayName("Last Date")]
        public DateTime? LastDate { get; set; }

        [DisplayName("Official Email")]
        [UIHint("Email")]
        public string OfficialEmail { get; set; }

        [DisplayName("Official Phone No")]
        [UIHint("PhoneNumber")]
        public string OfficialPhoneNo { get; set; }

        [DisplayName("Employee Status")]
        public EmployeeStatus? EmployeeStatus { get; set; }

        public int SkillId { get; set; }
        public int CertificationId { get; set; }

        [DisplayName("Requires TimeSheet")]
        public bool RequiresTimeSheet { get; set; }

        public double Salary { get; set; }

        public string Bank { get; set; }

        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [DisplayName("PAN Card")]
        public string PANCard { get; set; }

        [DisplayName("Payment Mode")]
        public PaymentMode PaymentMode { get; set; }

        public List<UserDocument> UserDocuments { get; set; }
        public List<UserSkill> UserSkills { get; set; }
        public List<UserHobby> UserHobbies { get; set; }
        public List<UserCertification> UserCertifications { get; set; }
        public List<RoleMember> RoleMembers { get; set; }
        public List<Asset> Assets { get; set; }
        public List<LeaveEntitlement> LeaveEntitlements { get; set; }
        public List<ProjectMember> Projects { get; set; }
        public List<Technology> Technologies { get; set; }

        public List<LinkedAccount> LinkedAccounts { get; set; }

        public bool HasSoftwareInfo { get; set; }
        public List<SoftwareModel> Softwares { get; set; }

        public bool HasHardwareInfo { get; set; }
        public HardwareModel Hardware { get; set; }

        public List<EmergencyContact> EmergencyContacts { get; set; }
        public List<EmployeeDependent> EmployeeDependents { get; set; }
        public List<Entities.User> Reportees { get; set; }
        public List<Token> Tokens { get; set; }

        public List<UserAward> UserAwards { get; set; }
        public List<LeaveEntitlementUpdate> LeaveEntitlementUpdates { get; set; }
        #region Leave Entitlement
        public LeaveOperation LeaveOperation { get; set; }
        public float Count { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
        #endregion

        public Dictionary<string, int> GroupedAwards
        {
            get
            {
                var dict = new Dictionary<string, int>();
                var keyvalues = UserAwards.GroupBy(a => a.Award.Title).Select(s => new { s.Key, Value = s.Count() }).ToList();
                foreach (var value in keyvalues)
                {
                    dict.Add(value.Key, value.Value);
                }
                return dict;
            }
        }

        public UserViewModel()
        {
            UserDocuments = new List<UserDocument>();
            UserSkills = new List<UserSkill>();
            UserHobbies = new List<UserHobby>();
            UserCertifications = new List<UserCertification>();
            RoleMembers = new List<RoleMember>();
            Assets = new List<Asset>();
            LeaveEntitlements = new List<LeaveEntitlement>();
            Projects = new List<ProjectMember>();
            Technologies = new List<Technology>();
            Softwares = new List<SoftwareModel>();
            EmergencyContacts = new List<EmergencyContact>();
            EmployeeDependents = new List<EmployeeDependent>();
            Reportees = new List<Entities.User>();
            Tokens = new List<Token>();
            UserAwards = new List<UserAward>();
            LeaveEntitlementUpdates = new List<LeaveEntitlementUpdate>();
        }

        public UserViewModel(Entities.User user) : this()
        {
            Id = user.Id;
            EmployeeCode = user.EmployeeCode;
            Username = user.Username;
            AccessRuleId = user.AccessRuleId;
            AccessRule = user.AccessRule;
            PersonId = user.PersonId;
            Person = user.Person;
            DepartmentId = user.DepartmentId;
            Department = user.Department;
            LocationId = user.LocationId;
            Location = user.Location;
            SeatNo = user.SeatNo;
            DesignationId = user.DesignationId;
            Designation = user.Designation;
            ShiftId = user.ShiftId;
            Shift = user.Shift;
            ReportingPersonId = user.ReportingPersonId;
            ReportingPerson = user.ReportingPerson;
            Experience = user.Experience;
            DateOfJoin = user.DateOfJoin;
            ConfirmationDate = user.ConfirmationDate;
            LastDate = user.LastDate;
            DateOfResignation = user.DateOfResignation;
            OfficialEmail = user.OfficialEmail;
            OfficialPhoneNo = user.OfficialPhone;
            EmployeeStatus = user.EmployeeStatus;
            RequiresTimeSheet = user.RequiresTimeSheet;
            Salary = user.Salary;
            Bank = user.Bank;
            BankAccountNumber = user.BankAccountNumber;
            PANCard = user.PANCard;
            PaymentMode = user.PaymentMode;
        }
    }
}