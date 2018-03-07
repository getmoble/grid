using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.Payroll.Entities
{
    public class EmployeeSalary: EntityBase
    {
        public double Salary { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string PANCard { get; set; }
        public PaymentMode PaymentMode { get; set; }
    }
}
