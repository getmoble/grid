using System.Collections.Generic;
using Grid.Infrastructure;

namespace Grid.Generic.Api.Controller
{
    using Data;
    using Features.CRM.Entities;
    using Features.HRMS.Entities;
    using Features.HRMS.Entities.Enums;
    using Features.IMS.Entities;
    using Features.IMS.Entities.Enums;
    using Features.LMS.Entities;
    using Features.LMS.Entities.Enums;
    using Features.PMS.Entities;
    using Features.PMS.Entities.Enums;
    using Grid.Api.Models;
    using Grid.Api.Models.CRM;
    using Grid.Api.Models.HRMS;
    using Grid.Api.Models.IMS;
    using Grid.Api.Models.IT;
    using Grid.Api.Models.LMS;
    using Grid.Api.Models.PMS;
    using Models;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Http;

    public class GenericListController : BaseWebApiController
    {
        private readonly GridDataContext _dataContext;

        public GenericListController(GridDataContext dataContext)
        {

            _dataContext = dataContext;
        }

        [Route("generic/GenericList/grid/objects")]
        [WebApiExceptionFilter]
        public IHttpActionResult GetGridObjects()
        {
            var apiResult = new ApiResult<List<GridObject>>
            {
                Status = true,
                Message = "Success",
                Result = _dataContext.Holidays.ToList().Select(h => new GridObject(h)).ToList()
            };

            return Json(apiResult);
        }

        [Route("generic/GenericList/{type}")]
        [WebApiExceptionFilter]
        public IHttpActionResult Get(string type)
        {

            var entityType = type.ToLower();
            IList<GridObject> data = null;
            int skip;
            int top;
            return WrapIntoApiResult(() =>
            {
                switch (entityType)
                {
                    case "vendor":
                        data = _dataContext.Vendors.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "project":
                        var projecttitle = HttpContext.Current.Request.QueryString["projecttitle"];
                        var projectStatus = HttpContext.Current.Request.QueryString["status"];
                        var clientid = HttpContext.Current.Request.QueryString["clientid"];
                        var projectType = HttpContext.Current.Request.QueryString["projecttype"];

                        IQueryable<Project> projectList = _dataContext.Projects.Include("Client.Person").Include("ParentProject");
                       
                     
                        if (!string.IsNullOrEmpty(projecttitle) && projecttitle != "undefined")
                        {
                            projectList = projectList.Where(i => i.Title.Contains(projecttitle));
                        }
                        if (!string.IsNullOrEmpty(clientid) && clientid != "undefined")
                        {
                            var client = Convert.ToInt32(clientid);
                            if (client != 0)
                                projectList = projectList.Where(i => i.ClientId == client);
                        }
                        if (!string.IsNullOrEmpty(projectType) && projectType != "undefined")
                        {
                            var typeProject = (ProjectType)Enum.Parse(typeof(ProjectType), projectType, true);
                            projectList = projectList.Where(i => i.ProjectType == typeProject);
                        }
                        if (!string.IsNullOrEmpty(projectStatus) && projectStatus != "undefined")
                        {
                            var statusType = (ProjectStatus)Enum.Parse(typeof(ProjectStatus), projectStatus, true);
                            projectList = projectList.Where(i => i.Status == statusType);
                        }
                        else
                        {
                            projectList = projectList.Where(i => i.Status != ProjectStatus.Closed && i.Status != ProjectStatus.Cancelled);
                        }
                       
                        if (!WebUser.IsAdmin)
                        {                            
                            var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                            var projectMember = _dataContext.ProjectMembers.Where(l => l.EmployeeId == currentEmployee.Id).Select(m => m.ProjectId).Distinct().ToList();
                            projectList = projectList.Where(p => projectMember.Contains(p.Id) || p.IsPublic || p.CreatedByUserId == WebUser.Id);

                        }
                        var project = projectList.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new ProjectModel(h)).ToList();                       
                        data = project.Select(h => new GridObject(h)).ToList();
                        break;                   
                    case "projectmember":
                        var projectmember = HttpContext.Current.Request.QueryString["projectId"];

                        if (!string.IsNullOrEmpty(projectmember))
                        {
                            var projectId = Convert.ToInt32(projectmember);
                            if (projectId != 0)
                            {
                                var projectmembers = _dataContext.ProjectMembers.Include("MemberEmployee.User.Person").Include("Project").Include("ProjectMemberRole").Where(i => i.ProjectId == projectId);
                               
                                var projects = projectmembers.Where(u=> u.MemberStatus == MemberStatus.Active).OrderBy(i => i.MemberStatus).ToList().Select(h => new ProjectModel(h)).ToList();
                                data = projects.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "inactiveprojectmember":
                        var inactiveProjectmember = HttpContext.Current.Request.QueryString["projectId"];

                        if (!string.IsNullOrEmpty(inactiveProjectmember))
                        {
                            var projectId = Convert.ToInt32(inactiveProjectmember);
                            if (projectId != 0)
                            {
                                var projectmembers = _dataContext.ProjectMembers.Include("MemberEmployee.User.Person").Include("Project").Include("ProjectMemberRole").Where(i => i.ProjectId == projectId);

                                var projects = projectmembers.Where(u => u.MemberStatus == MemberStatus.InActive).OrderBy(i => i.MemberStatus).ToList().Select(h => new ProjectModel(h)).ToList();
                                data = projects.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "task":
                        var taskTitle = HttpContext.Current.Request.QueryString["title"];
                        var taskStatus = HttpContext.Current.Request.QueryString["status"];
                        var projectid = HttpContext.Current.Request.QueryString["projectid"];
                        var employeeid = HttpContext.Current.Request.QueryString["employeeid"];

                        IQueryable<Task> taskslists = _dataContext.Tasks.Include("Assignee.User.Person").Include("Project");

                        if (!string.IsNullOrEmpty(taskTitle) && taskTitle != "undefined")
                        {
                            taskslists = taskslists.Where(i => i.Title.Contains(taskTitle));
                        }
                        if (!string.IsNullOrEmpty(projectid) && projectid != "undefined")
                        {
                            var projects = Convert.ToInt32(projectid);
                            if (projects != 0)
                                taskslists = taskslists.Where(i => i.ProjectId == projects);
                        }
                        if (!string.IsNullOrEmpty(employeeid) && employeeid != "undefined")
                        {
                            var employees = Convert.ToInt32(employeeid);
                            if (employees != 0)
                                taskslists = taskslists.Where(i => i.AssigneeId == employees);
                        }
                        if (!string.IsNullOrEmpty(taskStatus) && taskStatus != "undefined")
                        {
                            var statusType = (ProjectTaskStatus)Enum.Parse(typeof(ProjectTaskStatus), taskStatus, true);
                            taskslists = taskslists.Where(i => i.TaskStatus == statusType);
                        }
                        if (!string.IsNullOrEmpty(taskStatus) && taskStatus != "undefined")
                        {
                            var statusType = (ProjectTaskStatus)Enum.Parse(typeof(ProjectTaskStatus), taskStatus, true);
                            taskslists = taskslists.Where(i => i.TaskStatus == statusType);
                        }
                        else
                        {
                            taskslists = taskslists.Where(p => !(p.TaskStatus == ProjectTaskStatus.Cancelled || p.TaskStatus == ProjectTaskStatus.Completed));
                        }
                        if (!WebUser.IsAdmin)
                        {
                            var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                            var projectMember = _dataContext.ProjectMembers.Where(l => l.EmployeeId == currentEmployee.Id).Select(m => m.ProjectId).Distinct().ToList();
                            taskslists = taskslists.Where(p => projectMember.Contains(p.ProjectId) || p.AssigneeId == currentEmployee.Id || p.CreatedByUserId == WebUser.Id && !(p.TaskStatus == ProjectTaskStatus.Cancelled || p.TaskStatus == ProjectTaskStatus.Completed));

                        }
                        
                        var tasks = taskslists.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new TaskModel(h)).ToList();
                        data = tasks.Select(h => new GridObject(h)).ToList();
                        break;
                    case "holiday":
                        var leaveType = HttpContext.Current.Request.QueryString["leaveType"];
                        var year = HttpContext.Current.Request.QueryString["year"];

                        IQueryable<Holiday> query = _dataContext.Holidays;

                        if (!string.IsNullOrEmpty(leaveType) && leaveType != "undefined")
                        {
                            var holidayType = (HolidayType)Enum.Parse(typeof(HolidayType), leaveType, true);
                            query = query.Where(i => i.Type == holidayType);
                        }
                        if (!string.IsNullOrEmpty(year) && year != "undefined")
                        {
                            var years = Convert.ToInt32(year);
                            query = query.Where(i => i.Date.Year == years);
                        }
                        var model = query.OrderBy(i => i.Date).ToList().Select(h => new HolidayModel(h)).ToList();
                        data = model.Select(h => new GridObject(h)).ToList();
                        break;
                    case "leavetimeperiod":
                        data = _dataContext.LeaveTimePeriods.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "leavetype":
                        data = _dataContext.LeaveTypes.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "entitlement":
                        var leaveEntitlement = HttpContext.Current.Request.QueryString["leaveEntitlement"];

                        if (!string.IsNullOrEmpty(leaveEntitlement))
                        {
                            var employeeId = Convert.ToInt32(leaveEntitlement);
                            if (employeeId != 0)
                            {
                                var leavePeriod = _dataContext.LeaveTimePeriods.Where(i => i.Start <= DateTime.UtcNow && i.End >= DateTime.UtcNow).FirstOrDefault(); ;
                                var entitlementData = _dataContext.LeaveEntitlements.Include("Employee").Include("Employee.User").Include("Employee.User.Person").Include("CreatedByUser.Person").Include("LeaveTimePeriod").Include("LeaveType").Where(i => i.EmployeeId == employeeId && i.LeaveTimePeriod.Id == leavePeriod.Id);
                                var entitlementUpdateList = entitlementData.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new LeaveEntitlementModel(h)).ToList();
                                data = entitlementUpdateList.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                        case "leavebalance":                       
                        if (PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                        {
                            var employeeLeave = HttpContext.Current.Request.QueryString["employeeId"];
                            var employeeDepartment = HttpContext.Current.Request.QueryString["employeeDepartment"];

                            IQueryable<Employee> employeeList = _dataContext.Employees.Include("User").Include("User.Person").Include("Department").Include("Designation");

                            if (!string.IsNullOrEmpty(employeeDepartment) && employeeDepartment != "undefined")
                            {
                                var departmentId = Convert.ToInt32(employeeDepartment);
                                if (departmentId != 0)
                                    employeeList = employeeList.Where(i => i.DepartmentId == departmentId);
                            }
                            if (!string.IsNullOrEmpty(employeeLeave) && employeeLeave != "undefined")
                            {
                                var employeeId = Convert.ToInt32(employeeLeave);
                                if (employeeId != 0)
                                    employeeList = employeeList.Where(i => i.Id == employeeId);
                            }
                            
                            var employeeData = employeeList.Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1).OrderBy(i => i.EmployeeCode).ToList().Select(h => new EmployeeModel(h)).ToList();
                            data = employeeData.Select(h => new GridObject(h)).ToList();
                        }
                        else
                        {
                            return new List<GridObject>();
                        }
                        break;
                    case "leaveentitlement":
                        var employeeEntitlement = HttpContext.Current.Request.QueryString["employeeId"];
                        var leaveTypeEntitlement = HttpContext.Current.Request.QueryString["leaveTypeId"];
                        var leaveTimePeriod = HttpContext.Current.Request.QueryString["leaveTimePeriodId"];
                        var allocatedBy = HttpContext.Current.Request.QueryString["allocatedBy"];
                        var  operation = HttpContext.Current.Request.QueryString["operation"];

                        IQueryable<LeaveEntitlementUpdate> entitlementList = _dataContext.LeaveEntitlementUpdates.Include("Employee.User.Person").Include("LeaveType").Include("LeaveTimePeriod").Include("CreatedByUser.Person");

                        if (!string.IsNullOrEmpty(employeeEntitlement) && employeeEntitlement != "undefined")
                        {
                            var employeeId = Convert.ToInt32(employeeEntitlement);
                            if (employeeId != 0)
                                entitlementList = entitlementList.Where(i => i.EmployeeId == employeeId);
                        }
                        if (!string.IsNullOrEmpty(leaveTypeEntitlement) && leaveTypeEntitlement != "undefined")
                        {
                            var leaveTypeId = Convert.ToInt32(leaveTypeEntitlement);
                            if (leaveTypeId != 0)
                                entitlementList = entitlementList.Where(i => i.LeaveTypeId == leaveTypeId);
                        }
                        if (!string.IsNullOrEmpty(leaveTimePeriod) && leaveTimePeriod != "undefined")
                        {
                            var leaveTimePeriodId = Convert.ToInt32(leaveTimePeriod);
                            if (leaveTimePeriodId != 0)
                                entitlementList = entitlementList.Where(i => i.LeaveTimePeriodId == leaveTimePeriodId);
                        }
                        if (!string.IsNullOrEmpty(allocatedBy) && allocatedBy != "undefined")
                        {
                            var AllocatedById = Convert.ToInt32(allocatedBy);
                            if (AllocatedById != 0)
                                entitlementList = entitlementList.Where(i => i.CreatedByUserId == AllocatedById);
                        }
                        if (!string.IsNullOrEmpty(operation) && operation != "undefined")
                        {
                            var statusType = (LeaveOperation)Enum.Parse(typeof(LeaveOperation), operation, true);
                            entitlementList = entitlementList.Where(i => i.Operation == statusType);
                        }
                        var leaveentitlement = entitlementList.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new LeaveEntitlementModel(h)).ToList();
                        data = leaveentitlement.Select(h => new GridObject(h)).ToList();
                        break;
                    case "entitlementhistorylist":
                        var entitlementhistory = HttpContext.Current.Request.QueryString["entitlementhistory"];

                        if (!string.IsNullOrEmpty(entitlementhistory))
                        {
                            var employeeId = Convert.ToInt32(entitlementhistory);
                            if (employeeId != 0)
                            {
                                var entitlementshistory = _dataContext.LeaveEntitlementUpdates.Include("Employee.User.Person").Include("LeaveType").Include("LeaveTimePeriod").Where(i => i.EmployeeId == employeeId);
                                var history = entitlementshistory.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new LeaveEntitlementModel(h)).ToList();
                                data = history.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "designation":
                        var department = HttpContext.Current.Request.QueryString["department"];
                        var band = HttpContext.Current.Request.QueryString["band"];


                        IQueryable<Designation> stringQuery = _dataContext.Designations;

                        if (!string.IsNullOrEmpty(department) && department != "undefined")
                        {
                            var departmentId = Convert.ToInt32(department);
                            if (departmentId != 0)
                                stringQuery = stringQuery.Where(i => i.DepartmentId == departmentId);
                        }

                        if (!string.IsNullOrEmpty(band) && band != "undefined")
                        {
                            var bandType = (Band)Enum.Parse(typeof(Band), band, true);
                            stringQuery = stringQuery.Where(i => i.Band == bandType);
                        }
                        var modelData = stringQuery.OrderBy(i => i.Department.Title).ThenBy(i => i.Title).ToList().Select(h => new DesignationModel(h)).ToList();
                        data = modelData.Select(h => new GridObject(h)).ToList();
                        break;
                    case "department":
                        data = _dataContext.Departments.OrderBy(i => i.Title).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "award":
                        data = _dataContext.Awards.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "role":
                        data = _dataContext.Roles.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "technologie":
                        data = _dataContext.Technologies.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "emailtemplate":
                        data = _dataContext.EmailTemplates.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "skill":
                        data = _dataContext.Skills.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "hobbie":
                        data = _dataContext.Hobbies.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "shift":
                        data = _dataContext.Shifts.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "certification":
                        data = _dataContext.Certifications.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "assetcategorie":
                        data = _dataContext.AssetCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "software":
                        var software = _dataContext.Softwares.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new SoftwareModel(h)).ToList();
                        data = software.Select(h => new GridObject(h)).ToList();
                        break;
                    case "softwarecategorie":
                        data = _dataContext.SoftwareCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "asset":
                        var departmentData = HttpContext.Current.Request.QueryString["departmentData"];
                        var category = HttpContext.Current.Request.QueryString["category"];
                        var vendor = HttpContext.Current.Request.QueryString["vendor"];
                        var userName = HttpContext.Current.Request.QueryString["userName"];
                        var assetState = HttpContext.Current.Request.QueryString["assetState"];
                        var tagNumber = HttpContext.Current.Request.QueryString["tagNumber"];
                        var serialNumber = HttpContext.Current.Request.QueryString["serialNumber"];
                        var modelNumber = HttpContext.Current.Request.QueryString["modelNumber"];
                        var title = HttpContext.Current.Request.QueryString["title"];

                        IQueryable<Asset> assetList = _dataContext.Assets.Include("AllocatedEmployee.User.Person").Include("Department");

                        if (!string.IsNullOrEmpty(departmentData) && departmentData != "undefined")
                        {
                            var departmentId = Convert.ToInt32(departmentData);
                            if (departmentId != 0)
                                assetList = assetList.Where(i => i.DepartmentId == departmentId);
                        }
                        if (!string.IsNullOrEmpty(vendor) && vendor != "undefined")
                        {
                            var vendorId = Convert.ToInt32(vendor);
                            if (vendorId != 0)
                                assetList = assetList.Where(i => i.VendorId == vendorId);
                        }
                        if (!string.IsNullOrEmpty(category) && category != "undefined")
                        {
                            var categoryId = Convert.ToInt32(category);
                            if (categoryId != 0)
                                assetList = assetList.Where(i => i.AssetCategoryId == categoryId);
                        }
                        if (!string.IsNullOrEmpty(assetState) && assetState != "undefined")
                        {
                            var state = (AssetState)Enum.Parse(typeof(AssetState), assetState, true);
                            assetList = assetList.Where(i => i.State == state);
                        }

                        if (!string.IsNullOrEmpty(userName) && userName != "undefined")
                        {
                            var userId = Convert.ToInt32(userName);
                            if (userId != 0)
                                assetList = assetList.Where(i => i.AllocatedEmployeeId == userId);
                        }
                        if (!string.IsNullOrEmpty(tagNumber) && tagNumber != "undefined")
                        {
                            assetList = assetList.Where(i => i.TagNumber.Contains(tagNumber));
                        }
                        if (!string.IsNullOrEmpty(serialNumber) && serialNumber != "undefined")
                        {
                            assetList = assetList.Where(i => i.SerialNumber.Contains(serialNumber));
                        }
                        if (!string.IsNullOrEmpty(modelNumber) && modelNumber != "undefined")
                        {
                            assetList = assetList.Where(i => i.ModelNumber.Contains(modelNumber));
                        }
                        if (!string.IsNullOrEmpty(title) && title != "undefined")
                        {
                            assetList = assetList.Where(i => i.Title.Contains(title));
                        }
                        var asset = assetList.OrderBy(i => i.TagNumber).ToList().Select(h => new AssetModel(h)).ToList();

                        data = asset.Select(h => new GridObject(h)).ToList();
                        break;
                         case "employeeassets":
                        var employeeasset = HttpContext.Current.Request.QueryString["employeeasset"];

                        if (!string.IsNullOrEmpty(employeeasset))
                        {
                            var employeeId = Convert.ToInt32(employeeasset);
                            if (employeeId != 0)
                            {
                                var assetEmploye = _dataContext.Assets.Include("AllocatedEmployee.User.Person").Include("Department").Include("AssetCategory").Where(i => i.AllocatedEmployeeId == employeeId);

                                var assetData = assetEmploye.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new AssetModel(h)).ToList();
                                data = assetData.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "assetallocationhistory":
                        var assethistory = HttpContext.Current.Request.QueryString["assethistory"];
                        if (!string.IsNullOrEmpty(assethistory))
                        {
                            var assetId = Convert.ToInt32(assethistory);
                            if (assetId != 0)
                            {
                                var allocationList = _dataContext.AssetAllocations.Include("AllocatedEmployee.User.Person").Where(i => i.AssetId == assetId);

                                var allocationListData = allocationList.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new AssetModel(h)).ToList();
                                data = allocationListData.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "location":
                        data = _dataContext.Locations.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "requirementcategorie":
                        data = _dataContext.RequirementCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "ticketcategorie":
                        data = _dataContext.TicketCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "ticketsubcategorie":
                        data = _dataContext.TicketSubCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "employee":
                        if (PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                        {
                            var employeeDepartment = HttpContext.Current.Request.QueryString["employeeDepartment"];
                            var employeeCode = HttpContext.Current.Request.QueryString["employeeCode"];
                            var employeeLocation = HttpContext.Current.Request.QueryString["employeeLocation"];
                            var employeeDesignation = HttpContext.Current.Request.QueryString["employeeDesignation"];
                            var employeeShift = HttpContext.Current.Request.QueryString["employeeShift"];
                            var employeeName = HttpContext.Current.Request.QueryString["employeeName"];
                            var employeeStatus = HttpContext.Current.Request.QueryString["employeeStatus"];

                            IQueryable<Employee> employeeList =
                                _dataContext.Employees.Include("User")
                                    .Include("User.Person")
                                    .Include("Department")
                                    .Include("Designation");

                            if (!string.IsNullOrEmpty(employeeDepartment) && employeeDepartment != "undefined")
                            {
                                var departmentId = Convert.ToInt32(employeeDepartment);
                                if (departmentId != 0)
                                    employeeList = employeeList.Where(i => i.DepartmentId == departmentId);
                            }
                            if (!string.IsNullOrEmpty(employeeLocation) && employeeLocation != "undefined")
                            {
                                var locationId = Convert.ToInt32(employeeLocation);
                                if (locationId != 0)
                                    employeeList = employeeList.Where(i => i.LocationId == locationId);
                            }
                            if (!string.IsNullOrEmpty(employeeDesignation) && employeeDesignation != "undefined")
                            {
                                var designationId = Convert.ToInt32(employeeDesignation);
                                if (designationId != 0)
                                    employeeList = employeeList.Where(i => i.DesignationId == designationId);
                            }
                            if (!string.IsNullOrEmpty(employeeShift) && employeeShift != "undefined")
                            {
                                var shiftId = Convert.ToInt32(employeeShift);
                                if (shiftId != 0)
                                    employeeList = employeeList.Where(i => i.ShiftId == shiftId);
                            }
                            if (!string.IsNullOrEmpty(employeeName) && employeeName != "undefined")
                            {
                                var employeeId = Convert.ToInt32(employeeName);
                                if (employeeId != 0)
                                    employeeList = employeeList.Where(i => i.Id == employeeId);
                            }
                            if (!string.IsNullOrEmpty(employeeCode) && employeeCode != "undefined")
                            {
                                employeeList = employeeList.Where(i => i.EmployeeCode.Contains(employeeCode));
                            }
                            if (!string.IsNullOrEmpty(employeeStatus) && employeeStatus != "undefined")
                            {
                                var statusType =
                                    (EmployeeStatus)Enum.Parse(typeof(EmployeeStatus), employeeStatus, true);
                                employeeList = employeeList.Where(i => i.EmployeeStatus == statusType && i.Id != 1);
                            }
                            else
                            {
                                // Default filter to active employees only
                                employeeList =
                                    employeeList.Where(i => i.EmployeeStatus != EmployeeStatus.Ex && i.Id != 1);
                            }

                            var employeeData =
                                employeeList.OrderByDescending(i => i.Id)
                                    .ToList()
                                    .Select(h => new EmployeeModel(h))
                                    .ToList();
                            data = employeeData.Select(h => new GridObject(h)).ToList();
                        }
                        else
                        {
                            return new List<GridObject>();
                        }

                        break;
                    case "leaves":
                        TeamType teamType = TeamType.Me;
                        var employee = HttpContext.Current.Request.QueryString["employee"];
                        var leave = HttpContext.Current.Request.QueryString["leave"];
                        var duration = HttpContext.Current.Request.QueryString["duration"];
                        var status = HttpContext.Current.Request.QueryString["status"];
                        var startDate = HttpContext.Current.Request.QueryString["startDate"];
                        var approver = HttpContext.Current.Request.QueryString["approver"];
                        var team = HttpContext.Current.Request.QueryString["team"];

                        IQueryable<Leave> queryString = _dataContext.Leaves.Include("RequestedForUser.User.Person").Include("Approver.User.Person").Include("LeaveType");

                        if (!string.IsNullOrEmpty(team) && team != "undefined")
                        {
                            teamType = (TeamType)Enum.Parse(typeof(TeamType), team, true);
                        }
                        if (!string.IsNullOrEmpty(employee) && employee != "undefined")
                        {
                            var employeeId = Convert.ToInt32(employee);
                            if (employeeId != 0)
                                queryString = queryString.Where(i => i.RequestedForUserId == employeeId);
                        }
                        if (!string.IsNullOrEmpty(leave) && leave != "undefined")
                        {
                            var leaveTypeId = Convert.ToInt32(leave);
                            if (leaveTypeId != 0)
                                queryString = queryString.Where(i => i.LeaveTypeId == leaveTypeId);
                        }

                        if (!string.IsNullOrEmpty(duration) && duration != "undefined")
                        {
                            var durations = (LeaveDuration)Enum.Parse(typeof(LeaveDuration), duration, true);
                            queryString = queryString.Where(i => i.Duration == durations);
                        }

                        if (!string.IsNullOrEmpty(status) && status != "undefined")
                        {
                            var leavestatus = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), status, true);
                            queryString = queryString.Where(i => i.Status == leavestatus);
                        }
                        if (!string.IsNullOrEmpty(startDate) && startDate != "undefined")
                        {
                            var startDateTime = Convert.ToDateTime(startDate);
                            queryString = queryString.Where(i => i.Start == startDateTime);
                        }

                        if (!string.IsNullOrEmpty(approver) && approver != "undefined")
                        {
                            var approverId = Convert.ToInt32(approver);
                            if (approverId != 0)
                                queryString = queryString.Where(i => i.ApproverId == approverId);
                        }
                        if (WebUser.Permissions.Contains(215))
                        {
                            if (teamType == TeamType.Me)
                            {
                                var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                                queryString = queryString.Where(l => l.RequestedForUserId == currentEmployee.Id);
                            }
                        }
                        else
                        {
                            var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                            if (teamType == TeamType.TeamMembers)
                            {
                                var myReportees = _dataContext.Employees.Where(l => l.ReportingPersonId == currentEmployee.Id).Select(i => i.Id).ToList();
                                queryString = queryString.Where(i => myReportees.Contains(i.RequestedForUserId.Value));
                            }
                            else
                            {
                                queryString = queryString.Where(l => l.RequestedForUserId == currentEmployee.Id);
                            }

                        }
                        var leaves = queryString.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new LeaveModel(h)).ToList();
                        data = leaves.Select(h => new GridObject(h)).ToList();
                        break;
                    case "employeedependent":
                        var employeeDependent = HttpContext.Current.Request.QueryString["employeeDependent"];

                        if (!string.IsNullOrEmpty(employeeDependent))
                        {
                            var employeeId = Convert.ToInt32(employeeDependent);
                            if (employeeId != 0)
                            {
                                var dependent = _dataContext.EmployeeDependents.Include("Employee.User.Person").Where(i => i.EmployeeId == employeeId);

                                var dependentData = dependent.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new EmployeeDependentmodel(h)).ToList();
                                data = dependentData.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "emergencycontact":
                        var emergencyContact = HttpContext.Current.Request.QueryString["emergencyContact"];

                        if (!string.IsNullOrEmpty(emergencyContact))
                        {
                            var employeeId = Convert.ToInt32(emergencyContact);
                            if (employeeId != 0)
                            {
                                var contact = _dataContext.EmergencyContacts.Include("Employee").Where(i => i.EmployeeId == employeeId);

                                var contactPerson = contact.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new EmergencyContactModel(h)).ToList();
                                data = contactPerson.Select(h => new GridObject(h)).ToList();
                            }
                        }
                        break;
                    case "crmleadsource":
                        data = _dataContext.CRMLeadSources.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmleadstatu":
                        data = _dataContext.CRMLeadStatuses.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmleadcategorie":
                        data = _dataContext.CRMLeadCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmpotentialcategorie":
                        data = _dataContext.CRMPotentialCategories.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmsalesstage":
                        data = _dataContext.CRMSalesStages.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmaccount":
                        var crmaccount = _dataContext.CRMAccounts.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new CRMAccountModel(h)).ToList();
                        data = crmaccount.Select(h => new GridObject(h)).ToList();
                        break;
                    case "crmcontact":
                        var account = HttpContext.Current.Request.QueryString["account"];

                        IQueryable<CRMContact> crmContactQuery = _dataContext.CRMContacts.Include("Person").Include("ParentAccount");

                        if (!string.IsNullOrEmpty(account) && account != "undefined")
                        {
                            var accountId = Convert.ToInt32(account);
                            if (accountId != 0)
                                crmContactQuery = crmContactQuery.Where(i => i.ParentAccountId == accountId);
                        }
                        var contactData = crmContactQuery.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new CRMContactModel(h)).ToList();
                        data = contactData.Select(h => new GridObject(h)).ToList();
                        break;
                    case "projectmemberrole":
                        var memberrole = _dataContext.ProjectMemberRoles.Include("Department").OrderByDescending(i => i.CreatedOn).ToList().Select(h => new ProjectMemberRoleModel(h)).ToList();
                        data = memberrole.Select(h => new GridObject(h)).ToList();
                        break;

                }

                skip = !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["skip"]) ? Convert.ToInt32(HttpContext.Current.Request.QueryString["skip"]) : 0;
                top = !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["top"]) ? Convert.ToInt32(HttpContext.Current.Request.QueryString["top"]) : 0;

                if (skip > 0 || top > 0)
                {
                    var enumerable = data.Skip(skip).Take(top);
                    return enumerable.ToList();
                }

                return data;
            });
        }

        [HttpGet, Route("generic/GenericList/Search/{type}")]
        [WebApiExceptionFilter]
        public IHttpActionResult Search(string type)
        {

            var entityType = type.ToLower();
            var result = new SearchResult<GridObject>();
            int page;
            int count;
            int totalCount;

            return WrapIntoApiResult(() =>
            {
                page = !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]) ? Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]) : 0;
                count = !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["count"]) ? Convert.ToInt32(HttpContext.Current.Request.QueryString["count"]) : 0;
                totalCount = 0;
                switch (entityType)
                {

                    case "leaves":
                        TeamType teamType = TeamType.Me;
                        var employee = HttpContext.Current.Request.QueryString["employee"];
                        var leave = HttpContext.Current.Request.QueryString["leave"];
                        var duration = HttpContext.Current.Request.QueryString["duration"];
                        var status = HttpContext.Current.Request.QueryString["status"];
                        var startDate = HttpContext.Current.Request.QueryString["startDate"];
                        var approver = HttpContext.Current.Request.QueryString["approver"];
                        var team = HttpContext.Current.Request.QueryString["team"];

                        IQueryable<Leave> queryString = _dataContext.Leaves.Include("RequestedForUser.User.Person").Include("Approver.User.Person");

                        if (!string.IsNullOrEmpty(team) && team != "undefined")
                        {
                            teamType = (TeamType)Enum.Parse(typeof(TeamType), team, true);
                        }
                        if (!string.IsNullOrEmpty(employee) && employee != "undefined")
                        {
                            var employeeId = Convert.ToInt32(employee);
                            if (employeeId != 0)
                                queryString = queryString.Where(i => i.RequestedForUserId == employeeId);
                        }
                        if (!string.IsNullOrEmpty(leave) && leave != "undefined")
                        {
                            var leaveTypeId = Convert.ToInt32(leave);
                            if (leaveTypeId != 0)
                                queryString = queryString.Where(i => i.LeaveTypeId == leaveTypeId);
                        }

                        if (!string.IsNullOrEmpty(duration) && duration != "undefined")
                        {
                            var durations = (LeaveDuration)Enum.Parse(typeof(LeaveDuration), duration, true);
                            queryString = queryString.Where(i => i.Duration == durations);
                        }

                        if (!string.IsNullOrEmpty(status) && status != "undefined")
                        {
                            var leavestatus = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), status, true);
                            queryString = queryString.Where(i => i.Status == leavestatus);
                        }
                        if (!string.IsNullOrEmpty(startDate) && startDate != "undefined")
                        {
                            var startDateTime = Convert.ToDateTime(startDate);
                            queryString = queryString.Where(i => i.Start == startDateTime);
                        }

                        if (!string.IsNullOrEmpty(approver) && approver != "undefined")
                        {
                            var approverId = Convert.ToInt32(approver);
                            if (approverId != 0)
                                queryString = queryString.Where(i => i.ApproverId == approverId);
                        }
                        if (WebUser.Permissions.Contains(215))
                        {
                            if (teamType == TeamType.Me)
                            {
                                var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                                queryString = queryString.Where(l => l.RequestedForUserId == currentEmployee.Id);
                            }
                        }
                        else
                        {
                            var currentEmployee = _dataContext.Employees.FirstOrDefault(l => l.UserId == WebUser.Id);
                            if (teamType == TeamType.TeamMembers)
                            {
                                var myReportees = _dataContext.Employees.Where(l => l.ReportingPersonId == currentEmployee.Id).Select(i => i.Id).ToList();
                                queryString = queryString.Where(i => myReportees.Contains(i.RequestedForUserId.Value));
                            }
                            else
                            {
                                queryString = queryString.Where(l => l.RequestedForUserId == currentEmployee.Id);
                            }

                        }
                        var leaves = queryString.OrderByDescending(i => i.CreatedOn).ToList().Select(h => new LeaveModel(h)).ToList();
                        result.Items = leaves.Select(h => new GridObject(h)).ToList();
                        totalCount = queryString.Count();
                        break;
                }
                if (count != 0)
                {
                    result.Items = result.Items.Skip(count * page)
                   .Take(count).ToList();
                }
                result.PagingInfo = new PagingInfo(page, count, totalCount);
                return result;
            });
        }

        #region Helpers

        private IQueryable<CardModel> BuildQuery(IQueryable<CardModel> query, int? skip, int? top)
        {
            query = query.OrderByDescending(p => p.CreatedOn);

            if (skip != null && skip.Value > 0)
                query = query.Skip(skip.GetValueOrDefault());
            if (top != null && (skip != null && top.Value > 0))
                query = query.Take(top.GetValueOrDefault());

            return query;
        }
        #endregion
    }
}
