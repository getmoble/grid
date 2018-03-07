using Grid.Api.Models;
using Grid.Api.Models.HRMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class EmployeeDependentsController : GridApiBaseController
    {
        private readonly IEmployeeDependentRepository _employeeDependentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDependentsController(IEmployeeDependentRepository employeeDependentRepository,
                                            IUnitOfWork unitOfWork)
        {
            _employeeDependentRepository = employeeDependentRepository;
            _unitOfWork = unitOfWork;
        }
        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _employeeDependentRepository.GetAll().Select(h => new EmployeeDependentmodel(h)).ToList();
            }, "Employee Dependents Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

      
        [HttpGet]   
        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() => _employeeDependentRepository.Get(id, "Employee,Employee.User,Employee.User.Person"), "Employee Dependent Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Update(EmployeeDependentmodel vm)
        {
            ApiResult<EmployeeDependent> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var dependent = new EmployeeDependent
                        {

                            Name = vm.Name,
                            DependentType = vm.DependentType,
                            Gender = vm.Gender,                          
                            EmployeeId = vm.EmployeeId,
                            Birthday = vm.Birthday,
                            Id = vm.Id
                        };
                        _employeeDependentRepository.Update(dependent);
                        _unitOfWork.Commit();
                        return dependent;
                    }, "Dependent updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var dependent = new EmployeeDependent
                        {
                            Name = vm.Name,
                            DependentType = vm.DependentType,                         
                            EmployeeId = vm.EmployeeId,
                            Gender = vm.Gender,
                            Birthday = vm.Birthday,
                            Id = vm.Id
                        };
                        _employeeDependentRepository.Create(dependent);
                        _unitOfWork.Commit();
                        return dependent;
                    }, "Dependent created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<EmployeeDependent>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
             
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _employeeDependentRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Employee Dependent deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
     
    }
}