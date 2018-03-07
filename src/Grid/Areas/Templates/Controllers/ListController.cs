using System.Collections.Generic;
using System.Web.Mvc;
using Grid.Areas.Templates.Models;
using Grid.Infrastructure;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.Common;

namespace Grid.Areas.Templates.Controllers
{
    public class ListController : GridBaseController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ListController(IProjectMemberRepository projectMemberRepository,                               
                              IEmployeeRepository employeeRepository,
                              IUnitOfWork unitOfWork)
        {

            
            _projectMemberRepository = projectMemberRepository;
            _employeeRepository = employeeRepository;          
            _unitOfWork = unitOfWork;
        }


        public ActionResult Index(string type, string mode = "Manage")
        {
            var fields = new List<UIField>();
            switch (type)
            {
                case "Projects":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Status", "Status"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Tickets":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("DueDate", "Due Date", UIFieldTypes.DateTime),
                        UIField.GetField("Status", "Status"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                case "TeamLeaves":
                    fields = new List<UIField>
                    {
                        UIField.GetField("CreatedByUser.Person.Name", "Employee"),
                        UIField.GetField("LeaveType.Title", "Leave Type"),
                        UIField.GetField("Duration"),
                        UIField.GetField("Start", "Start Date", UIFieldTypes.DateTime),
                        UIField.GetField("Start", "End Date", UIFieldTypes.DateTime),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime),
                        UIField.GetField("Status", "Status")
                    };
                    break;
                case "leaves":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Employee", "Employee"),
                        UIField.GetField("LeaveType", "Leave Type"),
                        UIField.GetField("Duration"),
                        UIField.GetField("Period", "Period"),                      
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.TimeAgo),
                        UIField.GetField("Status", "Status"),
                        UIField.GetField("Approver", "Approver"),
                    };

                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_LeaveListTemplate", fields);
                case "locations":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),                        
                        UIField.GetField("Phone"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "Permissions":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("PermissionCode"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "roles":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "technologies":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Icon"),
                        UIField.GetField("Category"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "awards":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "emailtemplates":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "departments":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("MailAlias", "Mail Alias"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "designations":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Department", "Department"),
                        UIField.GetField("MailAlias", "Mail Alias"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "shifts":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("StartTime", "Start Time", UIFieldTypes.DateTime),
                        UIField.GetField("EndTime", "End Time", UIFieldTypes.DateTime),
                        UIField.GetField("NeedsCompensation", "Needs Compensation"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "skills":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "hobbies":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "certifications":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "JobOpenings":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Vacancies", "Vacancy Count"),
                        UIField.GetField("Status", "Status"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "CandidateDesignations":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Rounds":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Offers":
                    fields = new List<UIField>
                    {
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "vendor":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title","Name"),
                        UIField.GetField("Email"),
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "holiday":
                    if (PermissionChecker.CheckPermission(ViewBag.Permissions as List<int>, 215))
                    {
                        ViewBag.IsManage = true;
                    }
                    fields = new List<UIField>
                    {
                        UIField.GetField("Type","Type"),
                        UIField.GetField("Title"),                        
                        UIField.GetField("Date", "Date", UIFieldTypes.DateTime)                      
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_HolidayListTemplate", fields);
                case "leavetype":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("MaxInAStretch", "Max in Stretch"),
                        UIField.GetField("MaxInMonth", "Max in Month"),
                        UIField.GetField("CanCarryForward", "Can Carry Forward"),
                        UIField.GetField("MaxCarryForward", "Max Carry Forward")
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "leavetimeperiod":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Start", "Start Date", UIFieldTypes.DateTime),
                        UIField.GetField("End", "End Date", UIFieldTypes.DateTime)                      
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "Vendors":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Email"),
                        UIField.GetField("Phone"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "assets":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title","Title"),
                        UIField.GetField("TagNumber","Tag"),                       
                        UIField.GetField("ModelNumber","Model"),
                        UIField.GetField("Department","Department"),
                        UIField.GetField("AllocatedEmployee","Allocated To"),
                    }; 
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_AssetListTemplate", fields);
                case "employeeassets":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title","Title"),
                        UIField.GetField("TagNumber","Tag"),
                        UIField.GetField("ModelNumber","Model"),
                        UIField.GetField("Department","Department"),
                        UIField.GetField("AssetCategory","AssetCategory"),
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_LeaveEntitlementsListTemplate", fields);
                case "assetallocationhistory":
                    fields = new List<UIField>
                    {
                        UIField.GetField("AllocatedEmployee","Allocated To"),
                        UIField.GetField("StateType","State"),
                        UIField.GetField("AllocatedOn", "Allocated On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ProjectMembersListTemplate", fields);
                case "assetcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "softwarecategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "softwares":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Version"),
                        UIField.GetField("StatusName","Status"),
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "CRMAccounts":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Industry"),
                        UIField.GetField("Email"),
                        UIField.GetField("PhoneNo"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "requirementcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "Categories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("IsPublic"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "ticketcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "ticketsubcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "Users":
                    fields = new List<UIField>
                    {
                        UIField.GetField("EmployeeCode", "Code"),
                        UIField.GetField("EmployeeStatus", "Status"),
                        UIField.GetField("RequiresTimeSheet", "TimeSheet"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Articles":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Category.Title", "Category"),
                        UIField.GetField("IsPublic"),
                        UIField.GetField("IsFeatured"),
                        UIField.GetField("Hits"),
                        UIField.GetField("Rating"),
                        UIField.GetField("Version"),
                        UIField.GetField("State"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    }; 
                    break;
                case "employees":            
                    fields = new List<UIField>
                    {
                        UIField.GetField("User","Name"),
                        UIField.GetField("EmployeeCode","Code"),                        
                        UIField.GetField("Department","Department"),
                        UIField.GetField("Designation","Designation"),                                                         
                    };
                    return PartialView("_EmployeeListTemplate", fields);
                case "dependents":
                    fields = new List<UIField>
                    {                      
                        UIField.GetField("Dependent", "Dependent Type"),
                        UIField.GetField("Name"),                       

                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "emergencycontacts":
                    fields = new List<UIField>
                    {                       
                        UIField.GetField("Name"),
                        UIField.GetField("RelationshipType", "Relationship")

                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "entitlementhistorylists":
                    fields = new List<UIField>
                    {
                        UIField.GetField("LeaveTimePeriod", "Time Period"),
                        UIField.GetField("LeaveType", "Leave Type"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)

                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_EntitlementhistoryListTemplate", fields);
                case "leaveentitlements":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Employee", "Employee"),
                        UIField.GetField("LeaveType", "Leave Type"),
                        UIField.GetField("LeaveTimePeriod", "Time Period"),
                        UIField.GetField("OperationType", "Operation"),
                        UIField.GetField("LeaveCount", "Leave Count"),
                        UIField.GetField("PreviousBalance", "Previous Balance"),
                        UIField.GetField("NewBalance", "New Balance"),
                        UIField.GetField("AllocatedBy", "Created By"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };

                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_LeaveEntitlementsHistoryListTemplate", fields);
                case "entitlements":
                    fields = new List<UIField>
                    {
                        UIField.GetField("LeaveTimePeriod", "Leave Cycle"),
                        UIField.GetField("LeaveType", "Leave Type"),                                              
                        UIField.GetField("Allocation", "Leave Balance"),
                    }; 
                    return PartialView("_LeaveEntitlementsListTemplate", fields);
                case "leavebalance":
                    fields = new List<UIField>
                    {
                        UIField.GetField("User","Name"),
                        UIField.GetField("EmployeeCode","Code"),
                        UIField.GetField("Department","Department"),
                        UIField.GetField("Designation","Designation"),
                    }; 
                    return PartialView("_HolidayListTemplate", fields);
                case "projects":
                    ViewBag.UserId = WebUser.IsAdmin;
                    bool isMember;
                    var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person");
                    isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
                    ViewBag.Role = isMember;                 
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("StatusType", "Status"),
                        UIField.GetField("ProjecttypeType", "Type"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ProjectListTemplate", fields);
                case "projectmembers":
                    ViewBag.UserId = WebUser.IsAdmin;
                    bool isProjectLead;
                    var user = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,Location,Department,Designation,Shift");
                    isProjectLead = _projectMemberRepository.Any(m => m.EmployeeId == user.Id && m.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
                    ViewBag.Role = isProjectLead;
                    fields = new List<UIField>
                    {
                        UIField.GetField("MemberEmployee", "Employee"),
                        UIField.GetField("RoleAndStatus", "Role & Status"),                        
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ProjectMembersListTemplate", fields);
                case "tasks":
                    ViewBag.UserId = WebUser.IsAdmin;
                    bool isVisible;
                    var loginUser = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person");
                    isVisible = _projectMemberRepository.Any(m => m.EmployeeId == loginUser.Id && (m.Role == MemberRole.ProjectManager || m.Role == MemberRole.Lead) ) || WebUser.IsAdmin;
                    ViewBag.IsVisible = isVisible;
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Project", "Project"),
                        UIField.GetField("Assignee", "Assignee")
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_TaskListTemplate", fields);

                case "leadsources":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "leadstatuses":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "leadcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "potentialcategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn","Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "salesstages":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn","Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "crmaccounts":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Industry", "Industry"),
                        UIField.GetField("Email", "Email"),
                        UIField.GetField("PhoneNo", "Phone No"),
                        UIField.GetField("CreatedOn","Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
                case "crmcontacts":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name","Name"),
                        UIField.GetField("ParentAccount", "Account"),
                        UIField.GetField("PhoneNo", "Phone No"),
                        UIField.GetField("Email", "Email"),
                        UIField.GetField("CreatedOn","Created On", UIFieldTypes.DateTime)
                    };
                    ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
                    return PartialView("_ListTemplate", fields);
            }

            ViewBag.Mode = mode == "Manage" ? "Manage" : "ReadOnly";
            return PartialView("_Table", fields);
        }
    }
}