namespace Grid.Features.HRMS.ViewModels.User
{
    public class EditUserViewModel: NewUserViewModel
    {
        public EditUserViewModel()
        {
            
        }

        public EditUserViewModel(Features.HRMS.Entities.User user)
        {
            Id = user.Id;
            EmployeeCode = user.EmployeeCode;
            Username = user.Username;
            Password = user.Password;
            AccessRuleId = user.AccessRuleId;
            PersonId = user.PersonId;
            Person = user.Person;
            DepartmentId = user.DepartmentId;
            LocationId = user.LocationId;
            DesignationId = user.DesignationId;
            ShiftId = user.ShiftId;
            ReportingPersonId = user.ReportingPersonId;
            Experience = user.Experience;
            DateOfJoin = user.DateOfJoin;
            ConfirmationDate = user.ConfirmationDate;
            DateOfResignation = user.DateOfResignation;
            LastDate = user.LastDate;
            OfficialEmail = user.OfficialEmail;
            OfficialPhone = user.OfficialPhone;
            OfficialMessengerId = user.OfficialMessengerId;
            EmployeeStatus = user.EmployeeStatus;
            RequiresTimeSheet = user.RequiresTimeSheet;
            CreatedOn = user.CreatedOn;
            Salary = user.Salary;
            Bank = user.Bank;
            BankAccountNumber = user.BankAccountNumber;
            PANCard = user.PANCard;
            PaymentMode = user.PaymentMode;
        }
    }
}