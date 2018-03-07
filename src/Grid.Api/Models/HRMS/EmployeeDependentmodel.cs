using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using System;

namespace Grid.Api.Models.HRMS
{
    public class EmployeeDependentmodel : ApiModelBase
    {
        public string Name { get; set; }           
        public DependentType DependentType { get; set; }
        public string Dependent { get; set; }
        public Gender Gender { get; set; }
        public string GenderType { get; set; }
        public DateTime Birthday { get; set; }     
        public string Employee { get; set; }
        public int? EmployeeId { get; set; }
        public EmployeeDependentmodel()
        {

        }
        public EmployeeDependentmodel(EmployeeDependent employeeDependent)
        {
            Id = employeeDependent.Id;
            Name = employeeDependent.Name;
            Dependent = GetEnumDescription(employeeDependent.DependentType);
            GenderType = GetEnumDescription(employeeDependent.Gender);         
            DependentType = employeeDependent.DependentType;         
            EmployeeId = employeeDependent.EmployeeId;
            Birthday = employeeDependent.Birthday;
        }
    }
}