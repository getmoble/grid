using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Grid.Data;
using Common.Utilities;
using Grid.Features.LMS.Entities.Enums;
using Grid.Generic.Api.Models;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.IT.Entities.Enums;
using Grid.Features.IMS.Entities.Enums;
using System.Data.Entity;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.CRM.Entities.Enums;

namespace Grid.Generic.Api.Controller
{

    public class SelectListController : BaseWebApiController
    {
        private readonly GridDataContext _dataContext;

        public SelectListController(GridDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static IEnumerable<SelectItem> ToSelectList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectItem { Name = Enum.GetName(typeof(T), e), Id = e }).ToList();
        }

        [Route("generic/SelectList/{type}")]
        [WebApiExceptionFilter]
        public IHttpActionResult Get(string type)
        {
            var itemType = type.ToLower();
            IList<SelectItem> data = null;
            return WrapIntoApiResult(() =>
            {
                switch (itemType)
                {
                    case "holidaytype":
                        return ToSelectList<HolidayType>().ToList();
                    case "leavetype":
                        data = _dataContext.LeaveTypes.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "duration":
                        return ToSelectList<LeaveDuration>().ToList();
                    case "teamtype":
                        return ToSelectList<TeamType>().ToList();
                    case "teammember":
                        var teammember = _dataContext.Users.Include("Person").Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1).ToList().OrderBy(t => t.Person.Name).ToList();
                        data = teammember.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Person.Name
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "team":
                        if (WebUser.Permissions.Contains(215))
                        {
                            var allEmployee = _dataContext.Employees.Include(p => p.User).Include(o => o.User.Person).Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1).ToList().OrderBy(t => t.User.Person.Name).ToList();
                            data = allEmployee.Select(i => new SelectItem
                            {
                                Id = i.Id,
                                Name = i.User.Person.Name
                            }).OrderBy(i => i.Name).ToList();
                        }
                        else
                        {
                            var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                            var myReportees = _dataContext.Employees.Include(p => p.User).Include(o => o.User.Person).Where(l => l.ReportingPersonId == currentEmployee.Id && l.EmployeeStatus != EmployeeStatus.Ex && l.Id != 1).ToList().OrderBy(t => t.User.Person.Name).ToList();
                            data = myReportees.Select(i => new SelectItem
                            {
                                Id = i.Id,
                                Name = i.User.Person.Name
                            }).OrderBy(i => i.Name).ToList();
                        }

                        break;
                    case "employee":
                        var member = _dataContext.Employees.Include(p => p.User).Include(o => o.User.Person).Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1).ToList().OrderBy(t => t.User.Person.Name).ToList();
                        data = member.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.User.Person.Name
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "employeeuser":
                        var user = _dataContext.Employees.Include(p => p.User).Include(o => o.User.Person).Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1).ToList().OrderBy(t => t.User.Person.Name).ToList();
                        data = user.Select(i => new SelectItem
                        {
                            Id = i.UserId,
                            Name = i.User.Person.Name
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "status":
                        return ToSelectList<LeaveStatus>().ToList();
                    case "department":
                        data = _dataContext.Departments.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "band":
                        return ToSelectList<Band>().ToList();
                    case "permission":
                        data = _dataContext.Permissions.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "technology":
                        data = _dataContext.Technologies.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "role":
                        data = _dataContext.Roles.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Name
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "softwarestatus":
                        return ToSelectList<SoftwareStatus>().ToList();
                    case "licensetype":
                        return ToSelectList<LicenseType>().ToList();
                    case "softwarecategory":
                        data = _dataContext.SoftwareCategories.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "ticketcategory":
                        data = _dataContext.TicketCategories.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "assetstate":
                        return ToSelectList<AssetState>().ToList();
                    case "assetcategory":
                        data = _dataContext.AssetCategories.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "vendor":
                        data = _dataContext.Vendors.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "gender":
                        return ToSelectList<Gender>().ToList();
                    case "designation":
                        data = _dataContext.Designations.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "employeestatus":
                        return ToSelectList<EmployeeStatus>().ToList();
                    case "maritalstatus":
                        return ToSelectList<MaritalStatus>().ToList();
                    case "bloodgroup":
                        return ToSelectList<BloodGroup>().ToList();
                    case "location":
                        data = _dataContext.Locations.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "shift":
                        data = _dataContext.Shifts.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "paymentmode":
                        return ToSelectList<PaymentMode>().ToList();
                    case "dependenttype":
                        return ToSelectList<DependentType>().ToList();
                    case "relationship":
                        return ToSelectList<Relationship>().ToList();
                    case "leavetimeperiod":
                        data = _dataContext.LeaveTimePeriods.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "leaveoperation":
                        return ToSelectList<LeaveOperation>().ToList();
                    case "project":
                        var project = _dataContext.Projects.OrderBy(t => t.Title).ToList();
                        data = project.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "activeproject":
                        var activeProject = _dataContext.Projects.Where(i => !(i.Status == ProjectStatus.Closed || i.Status == ProjectStatus.Cancelled)).ToList().OrderBy(t => t.Title).ToList();
                        data = activeProject.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "client":
                        var client = _dataContext.CRMContacts.Include("Person").ToList();
                        data = client.Select(i => new SelectItem                        
                        {
                            Id = i.Id,
                            Name = i.Person.Name
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "projectstatus":
                       return ToSelectList<ProjectStatus>().ToList();
                    case "memberrole":
                        var memberrole = Enum.GetValues(typeof(MemberRole)).Cast<MemberRole>();
                        data = memberrole.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "billing":
                        var billing = Enum.GetValues(typeof(Billing)).Cast<Billing>();
                        data = billing.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "taskstatus":
                        var taskstatus = Enum.GetValues(typeof(ProjectTaskStatus)).Cast<ProjectTaskStatus>();
                        data = taskstatus.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "priority":
                        return ToSelectList<TaskPriority>().ToList();
                    case "taskbilling":
                        var taskbilling = Enum.GetValues(typeof(TaskBilling)).Cast<TaskBilling>();
                        data = taskbilling.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "memberstatus":
                        return ToSelectList<MemberStatus>().ToList();
                    case "projecttype":
                        var projecttype = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>();
                        data = projecttype.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "salestatus":
                        var salestatus = Enum.GetValues(typeof(SaleStatus)).Cast<SaleStatus>();
                        data = salestatus.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "employeecount":
                        var employeecount = Enum.GetValues(typeof(EmployeeCount)).Cast<EmployeeCount>();
                        data = employeecount.Select(i => new SelectItem
                        {
                            Name = EnumUtils.GetEnumDescription(i),
                            Id = (int)i
                        }).ToList();
                        break;
                    case "crmaccounts":
                        var crmaccounts = _dataContext.CRMAccounts.ToList();
                        data = crmaccounts.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;
                    case "projectmemberroles":
                        var memberroles = _dataContext.ProjectMemberRoles.ToList();
                        data = memberroles.Select(i => new SelectItem
                        {
                            Id = i.Id,
                            Name = i.Title
                        }).OrderBy(i => i.Name).ToList();
                        break;

                }
                return data;
            });
        }
    }
}

