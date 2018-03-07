using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Areas.HRMS.Controllers
{
    public class HRMSDashboardController : GridBaseController
    {
        private readonly IUserRepository _userRepository;
        public HRMSDashboardController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region AjaxCalls
        public FileContentResult EmployeesByLocationCSV()
        {
            var csv = new StringBuilder();
            var employeesByLocation = _userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Location")
                                      .GroupBy(l => l.Location.Title)
                                      .Select(x => new
                                      {
                                          Key = x.Key,
                                          Value = x.Count()
                                      })
                                      .ToList();

            var keys = string.Join(",", employeesByLocation.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByLocation.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EmployeesByLocationCSV.csv");
        }
        public FileContentResult EmployeesByDepartmentCSV()
        {
            var csv = new StringBuilder();
            var employeesByDepartment = _userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Department")
                                  .GroupBy(l => l.Department.Title)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", employeesByDepartment.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByDepartment.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EmployeesByDepartmentCSV.csv");
        }
        public FileContentResult EmployeesByStatusCSV()
        {
            var csv = new StringBuilder();
            var employeesByStatus = _userRepository.GetAllBy(u => u.Id != 1)
                                  .GroupBy(l => l.EmployeeStatus)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", employeesByStatus.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByStatus.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EmployeesByStatusCSV.csv");
        }
        public FileContentResult EmployeesByGenderCSV()
        {
            var csv = new StringBuilder();
            var employeesByGender = _userRepository.GetAllBy(u => u.Id != 1, "Person")
                                  .GroupBy(l => l.Person.Gender)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", employeesByGender.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByGender.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EmployeesByGenderCSV.csv");
        }
        #endregion
        
        public ActionResult Index()
        {
            return View();
        }
    }
}