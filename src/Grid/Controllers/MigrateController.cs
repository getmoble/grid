using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Data;
using Grid.Features.HRMS.Entities;
using Grid.Models;

namespace Grid.Controllers
{
    public class MigrateController : Controller
    {
        private readonly GridDataContext _context;
        public MigrateController(GridDataContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var existing = _context.Employees.ToList();
            foreach (var employee in existing)
            {
                _context.Employees.Remove(employee);
            }

            _context.SaveChanges();

            // Without Managers
            var users = _context.Users.Where(u => u.ReportingPersonId == null).ToList();
            SaveEmployees(users);

            for (int i = 0; i < 70; i++)
            {
                // With Managers
                var subEmployees = _context.Users.Where(u => u.ReportingPersonId == i).ToList();
                SaveEmployees(subEmployees);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void SaveEmployees(List<User> users)
        {
            foreach (var user in users)
            {
                var employee = new Employee()
                {
                    Id = user.Id,
                    EmployeeCode = user.EmployeeCode,
                    UserId = user.Id,
                    DepartmentId = user.DepartmentId,
                    LocationId = user.LocationId,
                    SeatNo = user.SeatNo,
                    DesignationId = user.DesignationId,
                    ShiftId = user.ShiftId,
                    Salary = user.Salary,
                    Bank = user.Bank,
                    BankAccountNumber = user.BankAccountNumber,
                    PANCard = user.PANCard,
                    PaymentMode = user.PaymentMode,
                    ReportingPersonId = user.ReportingPersonId,
                    ManagerId = user.ManagerId,
                    Experience = user.Experience,
                    DateOfJoin = user.DateOfJoin,
                    ConfirmationDate = user.ConfirmationDate,
                    DateOfResignation = user.DateOfResignation,
                    LastDate = user.LastDate,
                    OfficialEmail = user.OfficialEmail,
                    OfficialPhone = user.OfficialPhone,
                    OfficialMessengerId = user.OfficialMessengerId,
                    EmployeeStatus = user.EmployeeStatus,
                    RequiresTimeSheet = user.RequiresTimeSheet,
                    Code = user.Code,
                    CreatedOn = user.CreatedOn
                };

                _context.Employees.Add(employee);
                _context.SaveChanges();
            }
        }
    }
}