using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class GraphController : GridApiBaseController
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
        private readonly ITimeSheetRepository _timeSheetRepository;

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GraphController(IProjectRepository projectRepository,
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
                                  ITimeSheetRepository timeSheetRepository,

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
            _timeSheetRepository = timeSheetRepository;
            _technologyRepository = technologyRepository;
            _crmContactRepository = crmContactRepository;
            _unitOfWork = unitOfWork;
        }

        //previous code for Timesheet graph
        //public  List<TimeSheetLineItem> EffortByDate(int projectId, string startdate, string enddate)
        //{

        //    var lineItems1 = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet,TimeSheet.CreatedByUser.Person").ToList();
        //    var lineItems = lineItems1.GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId, x.TimeSheet.CreatedByUser.Person.Name })
        //                               .Select(x => new
        //                               {
        //                                   Value = lineItems1.Sum(y => y.Effort),
        //                                   Id = x.Key.CreatedByUserId,
        //                                   Day = (DateTime)x.Key.Date,
        //                                   Name = x.Key.Name,
        //                                   Individual = x.OrderBy(i => i.TimeSheet.CreatedByUserId).Sum(y => y.Effort),
        //                               })
        //                               .ToList();

        //    return lineItems1;

        //}
        public FileContentResult EffortByDateCSV(int projectId, string startdate, string enddate)
        {
            var csv = new StringBuilder();
            csv.AppendLine("x,Total,My Effort");
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");

            if (startdate != null && startdate != "")
            {
                DateTime startingDate = Convert.ToDateTime(startdate);
                var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet,TimeSheet.CreatedByUser")
                       .GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId })
                       .Select(x => new
                       {
                           Value = x.Sum(y => y.Effort),
                           Day = (DateTime)x.Key.Date,
                           Individual = x.OrderBy(i => i.TimeSheet.CreatedByUserId).Sum(y => y.Effort),

                       })
                       .ToList().Where(i => i.Day == startingDate);

                foreach (var lineItem in lineItems)
                {
                    var date = lineItem.Day;
                    csv.AppendLine($"{date}, {lineItem.Value},{lineItem.Individual}");

                }
            }
            else if (enddate != null && enddate != "")
            {
                DateTime endingDate = Convert.ToDateTime(enddate);
                var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet")
                       .GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId })
                       .Select(x => new
                       {
                           Value = x.Sum(y => y.Effort),
                           Day = (DateTime)x.Key.Date,
                           Individual = x.OrderBy(i => i.TimeSheet.CreatedByUserId).Sum(y => y.Effort),

                       })
                       .ToList().Where(i => i.Day == endingDate);

                foreach (var lineItem in lineItems)
                {
                    var date = lineItem.Day;
                    csv.AppendLine($"{date}, {lineItem.Value},{lineItem.Individual}");

                }
            }
            else
            {
                var lineItems1 = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet,TimeSheet.CreatedByUser.Person").ToList();

                var lineItems = lineItems1.GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId, x.TimeSheet.CreatedByUser.Person.Name })
                                        .Select(x => new
                                        {
                                            Value = lineItems1.Sum(y => y.Effort),
                                            Id = x.Key.CreatedByUserId,
                                            Day = (DateTime)x.Key.Date,
                                            Name = x.Key.Name,
                                            Individual = x.OrderBy(i => i.TimeSheet.CreatedByUserId).Sum(y => y.Effort),
                                        })
                                        .ToList();

                foreach (var lineItem in lineItems)
                {
                    var date = lineItem.Day.ToShortDateString();
                    csv.AppendLine($"{date}, {lineItem.Value},{lineItem.Individual}");

                }
            }
            if (startdate != null && startdate != "" && enddate != null && enddate != "")
            {
                DateTime dateStart = Convert.ToDateTime(startdate);
                DateTime dateEnd = Convert.ToDateTime(enddate);
                var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet")
                       .GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId })
                       .Select(x => new
                       {
                           Value = x.Sum(y => y.Effort),
                           Day = (DateTime)x.Key.Date,
                           Individual = x.Where(i => i.TimeSheet.CreatedByUserId == WebUser.Id).Sum(y => y.Effort),

                       })
                       .ToList().Where(d => d.Day >= dateStart && d.Day <= dateEnd);

                foreach (var lineItem in lineItems)
                {
                    var date = lineItem.Day;
                    csv.AppendLine($"{date}, {lineItem.Value},{lineItem.Individual}");

                }
            }

            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EffortByDateCSV.csv");
        }

    }
}
