using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Grid.Api.Models
{
    public class EmployeeModel : ApiModelBase
    {      
        public string EmployeeCode { get; set; }         
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }
        public string User { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }     
        public string AccessRule { get; set; }
        public int AccessRuleId { get; set; }       
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }   
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string CommunicationAddress { get; set; }
        public string PassportNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public BloodGroup? BloodGroup { get; set; }  
        public MaritalStatus? MaritalStatus { get; set; }
        public DateTime? MarriageAnniversary { get; set; }
        public string SecondaryEmail { get; set; }       
        public string Email { get; set; }
        public Gender? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public string SeatNo { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public int? ShiftId { get; set; }
        public string Shift { get; set; }
        public double Salary { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string PANCard { get; set; }        
        public int? ReportingPersonId { get; set; }
        public string ReportingPerson { get; set; }
        public int? ManagerId { get; set; }
        public string Manager { get; set; }
        public float? Experience { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public DateTime? LastDate { get; set; }
        public string OfficialEmail { get; set; }
        public string OfficialPhone { get; set; }
        public string OfficialMessengerId { get; set; }
        public string Status { get; set; }        
        public PaymentMode PaymentMode { get; set; }
        public EmployeeStatus? EmployeeStatus { get; set; }
        public bool RequiresTimeSheet { get; set; }
        public string Reportees { get; set; }
        public string TeamMembers { get; set; }
        public string PhotoPath { get; set; }
        public List<int> TechnologyIds { get; set; }
        public List<int> RoleIds { get; set; }
       

        public EmployeeModel()
        {

        }
        public EmployeeModel(Employee employee)
        {
            Id = employee.Id;
          
            if (employee.Department != null)
            {
                Department = employee.Department.Title;
            }
            if (employee.Designation != null)
            {
                Designation = employee.Designation.Title;
            }
            if (employee.Shift != null)
            {
                Shift = employee.Shift.Title;
            }
            if (employee.Location != null)
            {
                Location = employee.Location.Title;
            }
            if (employee.User.Person != null)
            {
                User = employee.User.Person.Name;
            }           
            if (employee.ReportingPerson?.User.Person != null)
            {
                ReportingPerson = employee.ReportingPerson.User.Person.Name;
            }
            if (employee.Manager?.User.Person != null)
            {
                Manager = employee.Manager.User.Person.Name;
            }
        
            Username = employee.User.Username;
            Password = employee.User.Password;                    
            EmployeeCode = employee.EmployeeCode;
            DepartmentId = employee.DepartmentId;
            LocationId = employee.LocationId;
            AccessRuleId = employee.User.AccessRuleId;
            ShiftId = employee.ShiftId;
            DesignationId = employee.DesignationId;
            ReportingPersonId = employee.ReportingPersonId;
            ManagerId = employee.ManagerId;
            EmployeeStatus = employee.EmployeeStatus;
            Status = GetEnumDescription(employee.EmployeeStatus);
            RequiresTimeSheet = employee.RequiresTimeSheet;         
            Experience = employee.Experience;
            PaymentMode = employee.PaymentMode;
            OfficialMessengerId = employee.OfficialMessengerId;
            OfficialPhone = employee.OfficialPhone;
            OfficialEmail = employee.OfficialEmail;
            LastDate = employee.LastDate;
            DateOfResignation = employee.DateOfResignation;
            ConfirmationDate = employee.ConfirmationDate;
            DateOfJoin = employee.DateOfJoin;
            SeatNo = employee.SeatNo;
            BankAccountNumber = employee.BankAccountNumber;
            PANCard = employee.PANCard;
            Bank = employee.Bank;
            Salary = employee.Salary;            
            FirstName = employee.User.Person.FirstName;
            MiddleName = employee.User.Person.MiddleName;
            LastName = employee.User.Person.LastName;
            PhoneNo = employee.User.Person.PhoneNo;
            Address = employee.User.Person.Address;
            CommunicationAddress = employee.User.Person.CommunicationAddress;
            PassportNo = employee.User.Person.PassportNo;
            SecondaryEmail = employee.User.Person.SecondaryEmail;
            Email = employee.User.Person.Email;
            DateOfBirth = employee.User.Person.DateOfBirth;
            MarriageAnniversary = employee.User.Person.MarriageAnniversary;                     
            BloodGroup = employee.User.Person.BloodGroup;           
            MaritalStatus = employee.User.Person.MaritalStatus;
            Gender = employee.User.Person.Gender;
            UserId = employee.UserId;
            EmployeeId = employee.Id;
            PersonId = employee.User.PersonId;
            PhotoPath = employee.User.Person.PhotoPath;



        }
    }
}

