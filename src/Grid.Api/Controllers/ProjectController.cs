using Grid.Api.Models;
using Grid.Api.Models.PMS;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class ProjectController : GridApiBaseController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectActivityRepository _projectActivityRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTechnologyMapRepository _projectTechnologyMapRepository;
        private readonly IProjectMileStoneRepository _projectMileStoneRepository;
        private readonly IProjectBillingRepository _projectBillingRepository;
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskEffortRepository _taskEffortRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IProjectRepository projectRepository,
                                  IProjectActivityRepository projectActivityRepository,
                                  IProjectMemberRepository projectMemberRepository,
                                  IProjectBillingRepository projectBillingRepository,
                                  ITaskRepository taskRepository,
                                  ITaskEffortRepository taskEffortRepository,
                                  IProjectTechnologyMapRepository projectTechnologyMapRepository,
                                  IProjectMileStoneRepository projectMileStoneRepository,
                                  IProjectDocumentRepository projectDocumentRepository,
                                  ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                  IUserRepository userRepository,
                                  IEmployeeRepository employeeRepository,

        ITechnologyRepository technologyRepository,
                                  ICRMContactRepository crmContactRepository,
                                  IUnitOfWork unitOfWork)
        {

            _projectRepository = projectRepository;
            _projectActivityRepository = projectActivityRepository;
            _projectBillingRepository = projectBillingRepository;
            _projectMemberRepository = projectMemberRepository;
            _taskRepository = taskRepository;
            _taskEffortRepository = taskEffortRepository;
            _projectTechnologyMapRepository = projectTechnologyMapRepository;
            _projectMileStoneRepository = projectMileStoneRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _projectDocumentRepository = projectDocumentRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _technologyRepository = technologyRepository;
            _crmContactRepository = crmContactRepository;
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

        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var project = _projectRepository.Get(id, "Client.Person,ParentProject");
                var projectVm = new ProjectModel(project);
                var projectTechnologyIds = _projectTechnologyMapRepository.GetAllBy(m => m.ProjectId == id).Select(m => m.TechnologyId).ToList();
                projectVm.TechnologyIds = projectTechnologyIds;
                return projectVm;
            }, "Project Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(ProjectModel vm)
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
                        project.Status = vm.Status.Value;
                        project.Billing = vm.Billing;
                        project.ExpectedBillingAmount = vm.ExpectedBillingAmount;
                        project.Rate = vm.Rate;
                        project.ParentId = vm.ParentId;
                        project.IsPublic = vm.IsPublic;
                        project.InheritMembers = vm.InheritMembers;
                        project.IsClosed = vm.IsClosed;
                        project.ProjectType = vm.ProjectType;
                        project.UpdatedByUserId = WebUser.Id;
                        project.UpdatedOn = DateTime.UtcNow;

                        _projectRepository.Update(project);
                        _unitOfWork.Commit();

                        // Remove the existing mapped Technologies 
                        var existingMaps = _projectTechnologyMapRepository.GetAllBy(m => m.ProjectId == project.Id).ToList();
                        foreach (var map in existingMaps)
                        {
                            _projectTechnologyMapRepository.Delete(map);
                        }

                        _unitOfWork.Commit();


                        // Map the New Technologies
                        if (vm.TechnologyIds != null)
                        {
                            foreach (var technologyId in vm.TechnologyIds)
                            {
                                var newMap = new ProjectTechnologyMap
                                {
                                    ProjectId = vm.Id,
                                    TechnologyId = technologyId
                                };

                                _projectTechnologyMapRepository.Create(newMap);
                            }



                            _unitOfWork.Commit();
                        }
                        return project;
                    }, "Project updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newProject = new Project
                        {
                            ClientId = vm.ClientId,
                            Title = vm.Title,
                            Description = vm.Description,
                            StartDate = vm.StartDate,
                            EndDate = vm.EndDate,
                            Status = vm.Status.Value,
                            Billing = vm.Billing,
                            ParentId = vm.ParentId,
                            IsPublic = vm.IsPublic,
                            InheritMembers = vm.InheritMembers,
                            IsClosed = vm.IsClosed,
                            CreatedByUserId = WebUser.Id,
                            ProjectType = vm.ProjectType
                        };
                        _projectRepository.Create(newProject);
                        _unitOfWork.Commit();

                        // Map the Technologies
                        if (vm.TechnologyIds != null)
                        {
                            foreach (var technologyId in vm.TechnologyIds)
                            {
                                var newMap = new ProjectTechnologyMap
                                {
                                    ProjectId = newProject.Id,
                                    TechnologyId = technologyId
                                };

                                _projectTechnologyMapRepository.Create(newMap);
                            }

                            _unitOfWork.Commit();
                        }

                        return newProject;
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
