using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Models;

namespace Grid.Api.Controllers
{
    public class DepartmentsController : GridApiBaseController
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IDepartmentRepository departmentRepository,
                                   IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _departmentRepository.GetAll(), "Departments Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _departmentRepository.Get(id, "Parent"), "Department Fetched sucessfully");            
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _departmentRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Department deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(DepartmentModel departmentModel)
        {
            ApiResult<Department> apiResult;

            if (ModelState.IsValid)
            {
                if (departmentModel.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var department = new Department {
                            Title = departmentModel.Title,
                            ParentId = departmentModel.ParentId,
                            Description = departmentModel.Description,
                            MailAlias = departmentModel.MailAlias,
                            Id= departmentModel.Id
                        };
                        _departmentRepository.Update(department);
                        _unitOfWork.Commit();
                        return department;
                    }, "Department updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var department = new Department
                        {
                            Title = departmentModel.Title,
                            ParentId = departmentModel.ParentId,
                            Description = departmentModel.Description,
                            MailAlias = departmentModel.MailAlias
                        };
                        _departmentRepository.Create(department);
                        _unitOfWork.Commit();
                        return department;
                    }, "Department created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Department>();               
            }
           
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
