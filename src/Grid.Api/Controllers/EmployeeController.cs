using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class EmployeeController : GridBaseController
    {
        private readonly IUserRepository _userRepository;

        public EmployeeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var employees = _userRepository.GetAllBy(e => e.EmployeeStatus != EmployeeStatus.Ex && e.Id != 1, "AccessRule,Department,Designation,Location,Person,ReportingPerson,Shift").ToList().Select(u => new EmployeeInfo(u)).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProfileImage(int id)
        {
            var selectedEmployee = _userRepository.Get(id, "Person");
            var payload = new
            {
                Name = selectedEmployee.Person.Name,
                ImageUrl = selectedEmployee.Person.PhotoPath
            };
            return Json(payload, JsonRequestBehavior.AllowGet);
        }
    }
}