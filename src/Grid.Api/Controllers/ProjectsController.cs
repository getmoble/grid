using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Api.Models.PMS;
using Grid.Features.HRMS.DAL.Interfaces;
using System.Linq;

namespace Grid.Api.Controllers
{
    public class ProjectsController: GridApiBaseController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IProjectRepository projectRepository, IEmployeeRepository employeeRepository,
                                   IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }
        
        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _projectRepository.GetAll().Select(h => new ProjectModel(h)).ToList();
            }, "Projects Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _projectRepository.Get(id), "Project fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Project vm)
        {
            ApiResult<Project> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var project = _projectRepository.Get(vm.Id);
                        project.ClientId = vm.ClientId;
                        project.Title = vm.Title;
                        project.Description = vm.Description;
                        project.StartDate = vm.StartDate;
                        project.EndDate = vm.EndDate;
                        project.Status = vm.Status;
                        project.Billing = vm.Billing;
                        project.ExpectedBillingAmount = vm.ExpectedBillingAmount;
                        project.Rate = vm.Rate;
                        project.ParentId = vm.ParentId;
                        project.IsPublic = vm.IsPublic;
                        project.InheritMembers = vm.InheritMembers;
                        project.IsClosed = vm.IsClosed;
                        project.UpdatedByUserId = WebUser.Id;
                        project.UpdatedOn = DateTime.UtcNow;

                        _projectRepository.Update(project);
                        _unitOfWork.Commit();
                        return project;
                    }, "Article updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _projectRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Project created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Project>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _projectRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Project deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
