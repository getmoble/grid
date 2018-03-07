using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Grid.Areas.Templates.Models;
using Grid.Data;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.IT.Entities.Enums;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.LMS.DAL.Interfaces;

namespace Grid.Areas.Templates.Controllers
{
    public class FormController : GridBaseController
    {
        private readonly GridDataContext _gridDataContext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRepository _leaveRepository;

        public FormController(GridDataContext gridDataContext, IEmployeeRepository employeeRepository, ILeaveRepository leaveRepository)
        {
            _gridDataContext = gridDataContext;
            _employeeRepository = employeeRepository;
            _leaveRepository = leaveRepository;

        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = e.ToString() })).ToList();
        }

        public ActionResult Index(string type)
        {
            var fields = new List<UIField>();
            switch (type)
            {
                case "Users":

                    ViewBag.DepartmentId = new SelectList(_gridDataContext.Departments, "Id", "Title");
                    ViewBag.Gender = GetEnumSelectList<Gender>();
                    ViewBag.BloodGroup = GetEnumSelectList<BloodGroup>();
                    ViewBag.MaritalStatus = GetEnumSelectList<MaritalStatus>();
                    ViewBag.LocationId = new SelectList(_gridDataContext.Locations, "Id", "Title");
                    ViewBag.DesignationId = new SelectList(_gridDataContext.Designations, "Id", "Title");
                    ViewBag.ShiftId = new SelectList(_gridDataContext.Designations, "Id", "Title");
                    ViewBag.ReportingPersonId = new SelectList(_gridDataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name");
                    ViewBag.EmployeeStatus = GetEnumSelectList<EmployeeStatus>();
                    ViewBag.PaymentMode = GetEnumSelectList<PaymentMode>();
                    ViewBag.ShiftId = new SelectList(_gridDataContext.Roles, "Id", "Name");

                    ViewData["TechnologyIds"] = new MultiSelectList(_gridDataContext.Technologies, "Id", "Title");
                    ViewData["RoleIds"] = new MultiSelectList(_gridDataContext.Roles, "Id", "Name");

                    fields = new List<UIField>
                    {
                        UIField.GetField("DepartmentId", "Department", UIFieldTypes.DropDown),
                        UIField.GetField("EmployeeCode", "Employee Code"),
                        UIField.GetField("Username"),
                        UIField.GetField("Password", "Password", UIFieldTypes.Password),
                        UIField.GetField("Person.FirstName", "First Name"),
                        UIField.GetField("Person.MiddleName", "Middle Name"),
                        UIField.GetField("Person.LastName", "Last Name"),
                        UIField.GetField("Person.Gender", "Gender", UIFieldTypes.DropDown, "Gender"),
                        UIField.GetField("Person.Email"),
                        UIField.GetField("Person.SecondaryEmail", "Secondary Email"),
                        UIField.GetField("Person.PhoneNo", "Phone No"),
                        UIField.GetField("Person.Address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("Person.CommunicationAddress", "Communication Address", UIFieldTypes.TextArea),
                        UIField.GetField("Person.PassportNo", "Passport No"),
                        UIField.GetField("Person.DateOfBirth", "Date Of Birth", UIFieldTypes.DateTime),
                        UIField.GetField("Person.BloodGroup", "Blood Group", UIFieldTypes.DropDown, "BloodGroup"),
                        UIField.GetField("Person.MaritalStatus", "Marital Status", UIFieldTypes.DropDown, "MaritalStatus"),
                        UIField.GetField("Person.MarriageAnniversary", "Marriage Anniversary", UIFieldTypes.DateTime),
                        UIField.GetField("LocationId", "Location", UIFieldTypes.DropDown),
                        UIField.GetField("DesignationId", "Designation", UIFieldTypes.DropDown),
                        UIField.GetField("ShiftId", "Shift", UIFieldTypes.DropDown),
                        UIField.GetField("ReportingPersonId", "Reporting Person", UIFieldTypes.DropDown),
                        UIField.GetField("Experience"),
                        UIField.GetField("DateOfJoin", "Date Of Joining", UIFieldTypes.DateTime),
                        UIField.GetField("ConfirmationDate", "Date Of Confirmation", UIFieldTypes.DateTime),
                        UIField.GetField("DateOfResignation", "Date Of Resignation", UIFieldTypes.DateTime),
                        UIField.GetField("LastDate", "Last Date", UIFieldTypes.DateTime),
                        UIField.GetField("OfficialEmail", "Official Email"),
                        UIField.GetField("OfficialPhone", "Official Phone"),
                        UIField.GetField("OfficialMessengerId", "Official Messenger Id"),
                        UIField.GetField("EmployeeStatus", "Employee Status", UIFieldTypes.DropDown),
                        UIField.GetField("TechnologyIds", "Technologies", UIFieldTypes.MultiSelectDropDown),
                        UIField.GetField("RequiresTimeSheet", "Requires TimeSheet", UIFieldTypes.CheckBox),
                        UIField.GetField("Salary"),
                        UIField.GetField("Bank"),
                        UIField.GetField("BankAccountNumber", "Bank Account Number"),
                        UIField.GetField("PANCard", "PAN Card"),
                        UIField.GetField("PaymentMode", "Payment Mode", UIFieldTypes.DropDown),
                        UIField.GetField("RoleIds", "Roles", UIFieldTypes.MultiSelectDropDown),
                    };
                    break;
                case "Projects":
                    ViewBag.ParentId = new SelectList(_gridDataContext.Projects, "Id", "Title");
                    ViewBag.ClientId = new SelectList(_gridDataContext.CRMContacts.Include("Person"), "Id", "Person.Name");
                    ViewData["TechnologyIds"] = new MultiSelectList(_gridDataContext.Technologies, "Id", "Title");
                    ViewBag.Status = GetEnumSelectList<ProjectStatus>();
                    fields = new List<UIField>
                    {
                        UIField.GetField("ParentId", "Sub Project Of", UIFieldTypes.DropDown),
                        UIField.GetField("ClientId", "Client", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("TechnologyIds", "Technologies", UIFieldTypes.MultiSelectDropDown),
                        UIField.GetField("StartDate", "Start Date", UIFieldTypes.DateTime),
                        UIField.GetField("EndDate", "End Date", UIFieldTypes.DateTime),
                        UIField.GetField("Status", "Status", UIFieldTypes.DropDown),
                        UIField.GetField("IsPublic", "Is Public", UIFieldTypes.CheckBox),
                        UIField.GetField("InheritMembers", "Inherit Members", UIFieldTypes.CheckBox)
                    };
                    break;                
                case "EmailTemplates":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Content", "Content", UIFieldTypes.HtmlEditor)
                    };
                    break;
                case "TicketCategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "TicketSubCategories":

                    ViewBag.TicketCategoryId = new SelectList(_gridDataContext.TicketCategories, "Id", "Title");
                    fields = new List<UIField>
                    {
                        UIField.GetField("TicketCategoryId", "Ticket Category", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };

                    break;
                case "Tickets":

                    ViewBag.TicketCategoryId = new SelectList(_gridDataContext.TicketCategories, "Id", "Title");
                    ViewBag.TicketSubCategoryId = new SelectList(_gridDataContext.TicketSubCategories, "Id", "Title");
                    fields = new List<UIField>
                    {
                        UIField.GetField("TicketCategoryId", "Ticket Category", UIFieldTypes.DropDown),
                        UIField.GetField("TicketSubCategoryId", "Ticket Sub Category", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("DueDate", "Due Date", UIFieldTypes.DateTime)
                    };

                    break;
                case "Technologies":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("Icon"),
                        UIField.GetField("Category")
                    };
                    break;
                case "Categories":
                    ViewBag.ParentCategoryId = new SelectList(_gridDataContext.Categories, "Id", "Title");
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("IsPublic", "Is Public", UIFieldTypes.CheckBox),
                        UIField.GetField("ParentCategoryId", "Parent Category", UIFieldTypes.DropDown)
                    };
                    break;
                case "Articles":
                    ViewBag.CategoryId = new SelectList(_gridDataContext.Categories, "Id", "Title");
                    fields = new List<UIField>
                    {
                        UIField.GetField("CategoryId", "Category", UIFieldTypes.DropDown),
                        UIField.GetField("IsPublic", "Is Public", UIFieldTypes.CheckBox),
                        UIField.GetField("Title"),
                        UIField.GetField("Summary", "Summary", UIFieldTypes.HtmlEditor),
                        UIField.GetField("Content", "Content", UIFieldTypes.HtmlEditor),
                        UIField.GetField("ArticleVersion", "Article Version"),
                        UIField.GetField("Keywords"),
                        UIField.GetField("IsFeatured", "Is Featured", UIFieldTypes.CheckBox),
                    };
                    break;
                case "RequirementCategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title")
                    };
                    break;
                case "SoftwareCategories":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "Softwares":
                    ViewBag.Status = GetEnumSelectList<SoftwareStatus>();
                    ViewBag.LicenseType = GetEnumSelectList<LicenseType>();
                    ViewBag.SoftwareCategoryId = new SelectList(_gridDataContext.SoftwareCategories, "Id", "Title");
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("Version"),
                        UIField.GetField("LatestVersion", "Latest Version"),
                        UIField.GetField("RecommendedVersion", "Recommended Version"),
                        UIField.GetField("Status", "Status", UIFieldTypes.DropDown),
                        UIField.GetField("LicenseType", "License Type", UIFieldTypes.DropDown),
                        UIField.GetField("LatestVersion", "Latest Version"),
                        UIField.GetField("SoftwareCategoryId", "Software Category", UIFieldTypes.DropDown),
                    };
                    break;
                case "Awards":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Holidays":
                    ViewBag.HolidayType = GetEnumSelectList<HolidayType>();
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("HolidayType", "Holiday Type", UIFieldTypes.DropDown),
                        UIField.GetField("Date", "Date", UIFieldTypes.DateTime)
                    };
                    break;
                case "LeaveTypes":
                    fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Designations":
                    ViewBag.DepartmentId = new SelectList(_gridDataContext.Designations, "Id", "Title");
                    ViewBag.Band = GetEnumSelectList<Band>();
                    fields = new List<UIField>
                    {
                        UIField.GetField("DepartmentId", "Department", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Band", "Band", UIFieldTypes.DropDown),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("MailAlias", "Mail Alias"),
                    };
                    break;
                case "Shifts":
                    ViewBag.DepartmentId = new SelectList(_gridDataContext.Designations, "Id", "Title");
                    ViewBag.Band = GetEnumSelectList<Band>();
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("StartTime", "Start Time", UIFieldTypes.DateTime),
                        UIField.GetField("EndTime", "End Time", UIFieldTypes.DateTime),
                        UIField.GetField("NeedsCompensation", "Needs Compensation", UIFieldTypes.CheckBox)
                    };
                    break;
                case "LeavePeriods":
                    fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                         UIField.GetField("Start", "Start Date", UIFieldTypes.DateTime),
                         UIField.GetField("End", "End Date", UIFieldTypes.DateTime)
                    };
                    break;
                case "Roles":

                    fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Locations":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Phone"),
                        UIField.GetField("Address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Skills":
                    fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Hobbies":
                    fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Certifications":
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "JobOpenings":
                    ViewBag.OpeningStatus = GetEnumSelectList<JobOpeningStatus>();
                    fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("NoOfVacancies", "No Of Vacancies"),
                        UIField.GetField("OpeningStatus", "Opening Status", UIFieldTypes.DropDown),
                    };
                    break;
            }

            return PartialView("_Bootstrap", fields);
        }

        public ActionResult FormContent(string type)
        {
            var fields = new List<UIField>();
            switch (type)
            {
                case "Employee":
                    ViewBag.Name = "employee";
                    fields = new List<UIField>
                    {
                        UIField.GetField("department","Department","department" ,"departmentId","Select Department" ,UIFieldTypes.DropDown),
                        UIField.GetField("employeeCode", "Employee Code"),
                        UIField.GetField("username","Username"),
                        UIField.GetField("password", "Password", UIFieldTypes.Password),
                        UIField.GetField("firstName", "First Name"),
                        UIField.GetField("middleName", "Middle Name"),
                        UIField.GetField("lastName", "Last Name"),
                        UIField.GetField("gender","Gender", "genderType","gender","Select Gender",UIFieldTypes.DropDown),
                        UIField.GetField("email","Email"),
                        UIField.GetField("secondaryEmail", "Secondary Email"),
                        UIField.GetField("phoneNo", "Phone No"),
                        UIField.GetField("address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("communicationAddress", "Communication Address", UIFieldTypes.TextArea),
                        UIField.GetField("passportNo", "Passport No"),
                        UIField.GetField("dateOfBirth", "Date Of Birth", UIFieldTypes.DateTime),
                        UIField.GetField("bloodgroup","Blood Group","bloodGroupType" ,"bloodGroup","Select Blood Group" ,UIFieldTypes.DropDown),
                        UIField.GetField("maritalstatus","Marital Status","maritalStatusType" ,"maritalStatus","Select Marital Status" ,UIFieldTypes.DropDown),
                        UIField.GetField("employeestatus","Employee Status","status" ,"employeeStatus","Select Employee Status" ,UIFieldTypes.DropDown),
                        UIField.GetField("marriageAnniversary", "Marriage Anniversary", UIFieldTypes.DateTime),
                        UIField.GetField("designation","Designation","designation" ,"designationId","Select Designation" ,UIFieldTypes.DropDown),
                        UIField.GetField("shift","Shift","shift" ,"shiftId","Select Shift" ,UIFieldTypes.DropDown),
                        UIField.GetField("location","Location","location" ,"locationId","Select Location" ,UIFieldTypes.DropDown),
                        UIField.GetField("employee","Reporting Person","reportingPerson" ,"reportingPersonId","Select Employee" ,UIFieldTypes.DropDown),
                        UIField.GetField("employee","Manager","manager" ,"managerId","Select Employee" ,UIFieldTypes.DropDown),
                        UIField.GetField("experience","Experience"),
                        UIField.GetField("dateOfJoin", "Date Of Joining", UIFieldTypes.DateTime),
                        UIField.GetField("confirmationDate", "Date Of Confirmation", UIFieldTypes.DateTime),
                        UIField.GetField("dateOfResignation", "Date Of Resignation", UIFieldTypes.DateTime),
                        UIField.GetField("lastDate", "Last Date", UIFieldTypes.DateTime),
                        UIField.GetField("officialEmail", "Official Email"),
                        UIField.GetField("officialPhone", "Official Phone"),
                        UIField.GetField("officialMessengerId", "Slack"),
                        UIField.GetField("requiresTimeSheet", "Requires TimeSheet"),
                        UIField.GetField("salary","Salary"),
                        UIField.GetField("bank","Bank"),
                        UIField.GetField("bankAccountNumber", "Bank Account Number"),
                        UIField.GetField("panCard", "PAN Card"),
                        UIField.GetField("paymentmode","Payment Mode","paymentType" ,"paymentMode","Select Payment Mode" ,UIFieldTypes.DropDown),
                        UIField.GetField("technology","Technologies","technologyNames" ,"technologyIds","Select Technologies" ,UIFieldTypes.DropDown),
                        UIField.GetField("role","Roles","roleNames" ,"roleIds","Select Roles" ,UIFieldTypes.DropDown),
                    };
                    return PartialView("_EmployeesTemplate", fields);
                case "Vendor":
                    ViewBag.Name = "vendor";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Name"),
                        UIField.GetField("email","Email"),
                        UIField.GetField("phone","Phone"),
                        UIField.GetField("webSite","Website"),
                        UIField.GetField("address","Address"),
                        UIField.GetField("contactPerson","Contact Person Name"),
                        UIField.GetField("contactPersonEmail","Contact Person Email"),
                        UIField.GetField("contactPersonPhone","Contact Person Phone"),
                    };
                    break;
                case "Holiday":
                    ViewBag.Name = "holiday";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("holidayType","Holiday Type","holidayType" ,"type","Select Holiday Type", UIFieldTypes.DropDown),
                        UIField.GetField("date", "Date", UIFieldTypes.DateTime),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "LeavePeriod":
                    ViewBag.Name = "leaveTimePeriod";
                    fields = new List<UIField>
                    {
                         UIField.GetField("title","Title"),
                         UIField.GetField("start", "Start Date", UIFieldTypes.DateTime),
                         UIField.GetField("end", "End Date", UIFieldTypes.DateTime),
                         UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "LeaveType":
                    ViewBag.Name = "leavetype";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("maxInAStretch", " Max In A Stretch", UIFieldTypes.Text),
                        UIField.GetField("maxInMonth", "Max In Month", UIFieldTypes.Text),
                        UIField.GetField("canCarryForward", "Can Carry Forward", UIFieldTypes.CheckBox),
                        UIField.GetField("maxCarryForward", "Max Carry Forward", UIFieldTypes.Text)
                    };
                    break;
                case "Leave":
                    ViewBag.Name = "leaves";
                    return PartialView("_LeavesTemplate");
                case "Designation":
                    ViewBag.Name = "designation";
                    fields = new List<UIField>
                    {

                         UIField.GetField("department","Department","department" ,"departmentId","Select Department" ,UIFieldTypes.DropDown),
                         UIField.GetField("title","Title",UIFieldTypes.Text),
                         UIField.GetField("band","Band", "bandName","band","Select Band",UIFieldTypes.DropDown),
                         UIField.GetField("mailAlias","Mail Alias",UIFieldTypes.Text),
                         UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Department":
                    ViewBag.Name = "department";
                    fields = new List<UIField>
                    {

                         UIField.GetField("title","Title",UIFieldTypes.Text),
                         UIField.GetField("department","Parent Department","parent" ,"parentId","Select Department", UIFieldTypes.DropDown),
                         UIField.GetField("mailAlias","Mail Alias",UIFieldTypes.Text),
                         UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Award":
                    ViewBag.Name = "award";
                    fields = new List<UIField>
                    {

                         UIField.GetField("title","Title",UIFieldTypes.Text),
                         UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Role":
                    ViewBag.Name = "role";
                    var permissions = UIField.GetField("permissionIds", "Permissions", UIFieldTypes.MultiSelectDropDown, "rolePermissions");
                    permissions.Binding = "options: $parent.permissions, selectedOptions: permissionIds, optionsText:'name',optionsValue:'id'";

                    fields = new List<UIField>
                    {
                        UIField.GetField("name","Name"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                        permissions,
                    };
                    break;
                case "Technology":
                    ViewBag.Name = "technologie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("category","Category"),
                        UIField.GetField("icon","Icon"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "EmailTemplate":
                    ViewBag.Name = "emailtemplate";
                    fields = new List<UIField>
                    {
                        UIField.GetField("name","Name"),
                        UIField.GetField("content", "Content", UIFieldTypes.HtmlEditor)
                    };
                    break;
                case "Skill":
                    ViewBag.Name = "skill";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Hobby":
                    ViewBag.Name = "hobbie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Shift":
                    ViewBag.Name = "shift";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("startTime", "Start Time", UIFieldTypes.DateTime),
                        UIField.GetField("endTime", "End Time", UIFieldTypes.DateTime),
                        UIField.GetField("needsCompensation", "Needs Compensation", UIFieldTypes.CheckBox),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "Certification":
                    ViewBag.Name = "certification";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "AssetCategory":
                    ViewBag.Name = "assetcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Software":
                    ViewBag.Name = "software";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("version","Version"),
                        UIField.GetField("latestVersion", "Latest Version"),
                        UIField.GetField("recommendedVersion", "Recommended Version"),
                        UIField.GetField("softwarestatus","Status", "statusType","status","Select Status",UIFieldTypes.DropDown),
                        UIField.GetField("licensetype","License Type", "license","licenseType","Select License Type",UIFieldTypes.DropDown),
                        UIField.GetField("softwarecategory","Software Category","category" ,"softwareCategoryId","Select Software Category", UIFieldTypes.DropDown),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "SoftwareCategory":
                    ViewBag.Name = "softwarecategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "Location":
                    ViewBag.Name = "location";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("phone","Phone"),
                        UIField.GetField("address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "RequirementCategory":
                    ViewBag.Name = "requirementcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title")
                    };
                    break;
                case "TicketCategory":
                    ViewBag.Name = "ticketcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "TicketSubCategory":
                    ViewBag.Name = "ticketsubcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("ticketcategory","Ticket Category","ticketCategory" ,"ticketCategoryId","Select Ticket Category", UIFieldTypes.DropDown),
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Asset":
                    ViewBag.Name = "asset";
                    fields = new List<UIField>
                    {
                        UIField.GetField("serialNumber","Serial Number",UIFieldTypes.Text),
                        UIField.GetField("title","Title",UIFieldTypes.Text),
                        UIField.GetField("cost","Cost",UIFieldTypes.Text),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("specifications", "Specifications", UIFieldTypes.TextArea),
                        UIField.GetField("brand","Brand",UIFieldTypes.Text),
                        UIField.GetField("modelNumber","Model Number",UIFieldTypes.Text),
                        UIField.GetField("isBrandNew", "Is Brand New", UIFieldTypes.CheckBox),
                        UIField.GetField("assetstate","State", "stateType","state","Select State",UIFieldTypes.DropDown),
                        UIField.GetField("purchaseDate", "Purchase Date", UIFieldTypes.DateTime),
                        UIField.GetField("warrantyExpiryDate", "Warranty Expiry Date", UIFieldTypes.DateTime),
                        UIField.GetField("assetcategory","Asset Category","assetCategory" ,"assetCategoryId","Select Asset Category", UIFieldTypes.DropDown),
                        UIField.GetField("department","Department","department" ,"departmentId","Select Department" ,UIFieldTypes.DropDown),
                        UIField.GetField("vendor","Vendor","vendor" ,"vendorId","Select Vendor" ,UIFieldTypes.DropDown),
                        UIField.GetField("employee","Allocated To","allocatedEmployee" ,"allocatedEmployeeId","Select Employee" ,UIFieldTypes.DropDown),
                    };
                    return PartialView("_AssetTemplate", fields);
                case "Dependent":
                    ViewBag.Name = "employeedependent";
                    fields = new List<UIField>
                    {
                        UIField.GetField("dependenttype","Dependent Type", "dependent","dependentType","Select Dependent",UIFieldTypes.DropDown),
                        UIField.GetField("name","Name"),
                        UIField.GetField("birthday", "Date of Birth", UIFieldTypes.DateTime),
                        UIField.GetField("gender","Gender", "genderType","gender","Select Gender",UIFieldTypes.DropDown),

                    };
                    break;
                case "EmergencyContact":
                    ViewBag.Name = "emergencycontact";
                    fields = new List<UIField>
                    {
                        UIField.GetField("relationship","Relationship", "relationshipType","relationship","Select Relationship",UIFieldTypes.DropDown),
                        UIField.GetField("name","Name",UIFieldTypes.Text),
                        UIField.GetField("mobile","Mobile",UIFieldTypes.Text),
                        UIField.GetField("phone","Phone",UIFieldTypes.Text),
                        UIField.GetField("workPhone","WorkPhone",UIFieldTypes.Text),
                        UIField.GetField("email","Email",UIFieldTypes.Text),
                        UIField.GetField("address", "Address", UIFieldTypes.TextArea),
                    };
                    break;
                case "LeaveEntitlement":
                    ViewBag.Name = "leaveentitlement";
                    fields = new List<UIField>
                    {
                        UIField.GetField("employee","Employee","employee" ,"employeeId","Select Employee" ,UIFieldTypes.DropDown),
                        UIField.GetField("leavetype","Leave Type","leaveType" ,"leaveTypeId","Select Leave Type", UIFieldTypes.DropDown),
                        UIField.GetField("leavetimeperiod","Time period","leaveTimePeriod" ,"leaveTimePeriodId","Select Time Period", UIFieldTypes.DropDown),
                        UIField.GetField("leaveoperation","Operation","operationType","operation","Select Operation", UIFieldTypes.DropDown),
                        UIField.GetField("allocation", "Allocated Days",UIFieldTypes.Text),
                        UIField.GetField("comments", "Comments", UIFieldTypes.TextArea),

                    };
                    return PartialView("_LeaveEntitlementsTemplate", fields);
                case "EntitlementHistory":
                    ViewBag.Name = "entitlementhistorylist";
                    fields = new List<UIField>
                    {
                        UIField.GetField("leavetype","Leave Type","leaveType" ,"leaveTypeId","Select Leave Type", UIFieldTypes.DropDown),
                        UIField.GetField("leavetimeperiod","Time period","leaveTimePeriod" ,"leaveTimePeriodId","Select Time Period", UIFieldTypes.DropDown),
                        UIField.GetField("leaveoperation","Operation","operationType","operation","Select Operation", UIFieldTypes.DropDown),
                        UIField.GetField("leaveCount", "Leave Count",UIFieldTypes.Text),
                        UIField.GetField("previousBalance", "Previous Balance",UIFieldTypes.Text),
                        UIField.GetField("newBalance", "New Balance",UIFieldTypes.Text),
                        UIField.GetField("comments", "Comments", UIFieldTypes.TextArea),

                    };
                    break;
                case "Project":
                    ViewBag.Name = "project";
                    return PartialView("_ProjectTemplate");
                case "ProjectMember":
                    ViewBag.Name = "projectmember";
                    ViewBag.UserId = WebUser.Id;
                    fields = new List<UIField>
                    {
                        UIField.GetField("employee","Employee","memberEmployee" ,"employeeId","Select Employee" ,UIFieldTypes.DropDown),
                        UIField.GetField("memberrole","Role","roleType" ,"role","Select Role", UIFieldTypes.DropDown),
                        UIField.GetField("billing","Billing","billingType" ,"billing","Select Billing", UIFieldTypes.DropDown),
                        UIField.GetField("memberstatus","Status","memberStatusType" ,"memberStatus","Select Status", UIFieldTypes.DropDown),
                        UIField.GetField("rate", "Rate",UIFieldTypes.Text),
                    };
                    return PartialView("_ProjectMemberTemplate", fields);
                case "Task":
                    ViewBag.Name = "task";
                    return PartialView("_TaskTemplate");
                case "LeaveBalance":
                    ViewBag.Name = "leavebalance";
                    return PartialView("_LeaveBalanceDetailsTemplate");
                case "Image":
                    return PartialView("_singleImageComponent");
                case "LeadSource":
                    ViewBag.Name = "crmleadsource";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break; 
                case "LeadStatus":
                    ViewBag.Name = "crmleadstatu";
                    fields = new List<UIField>
                    {
                        UIField.GetField("name","Name"),
                    };
                    break;
                case "LeadCategory":
                    ViewBag.Name = "crmleadcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "PotentialCategory":
                    ViewBag.Name = "crmpotentialcategorie";
                    fields = new List<UIField>
                    {
                        UIField.GetField("title","Title"),
                        UIField.GetField("description","Description",UIFieldTypes.TextArea)
                    };
                    break;
                case "SalesStage":
                    ViewBag.Name = "crmsalesstage";
                    fields = new List<UIField>
                    {
                        UIField.GetField("name","Name"),
                        UIField.GetField("salestatus","Status","statusType" ,"status","Select Status", UIFieldTypes.DropDown),
                    };
                    break;
                case "CRMAccounts":
                    ViewBag.Name = "crmaccount";
                    fields = new List<UIField>
                    {
                        UIField.GetField("employee","Assigned To User","assignedToEmployee" ,"assignedToEmployeeId","Select Employee" ,UIFieldTypes.DropDown),
                        UIField.GetField("title","Title",UIFieldTypes.Text),
                        UIField.GetField("industry","Industry",UIFieldTypes.Text),
                        UIField.GetField("employeecount","Employee Count", "employeeCountType","employeeCount","Select Employee Count",UIFieldTypes.DropDown),
                        UIField.GetField("foundedOn", "Founded On", UIFieldTypes.DateTime),
                        UIField.GetField("email","Email",UIFieldTypes.Text),
                        UIField.GetField("phoneNo","Phone No",UIFieldTypes.Text),
                        UIField.GetField("secondaryEmail","Secondary Email",UIFieldTypes.Text),
                        UIField.GetField("officePhone","Office Phone",UIFieldTypes.Text),
                        UIField.GetField("website","Website",UIFieldTypes.Text),
                        UIField.GetField("facebook","Facebook",UIFieldTypes.Text),
                        UIField.GetField("twitter","Twitter",UIFieldTypes.Text),
                        UIField.GetField("googlePlus","Google Plus",UIFieldTypes.Text),
                        UIField.GetField("linkedIn","Linked In",UIFieldTypes.Text),
                        UIField.GetField("city","City",UIFieldTypes.Text),
                        UIField.GetField("country","Country",UIFieldTypes.Text),
                        UIField.GetField("address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("communicationAddress", "Communication Address", UIFieldTypes.TextArea),
                        UIField.GetField("expertise","Expertise",UIFieldTypes.Text),
                        UIField.GetField("description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("crmaccounts","Parent Account","parent" ,"parentId","Select Parent Account" ,UIFieldTypes.DropDown),                 
                    };
                    break;
                case "CRMContacts":
                    ViewBag.Name = "crmcontact";
                    fields = new List<UIField>
                    {
                        UIField.GetField("crmaccounts","Parent Account","parentAccount" ,"parentAccountId","Select Account" ,UIFieldTypes.DropDown),
                        UIField.GetField("firstName","FirstName",UIFieldTypes.Text),
                        UIField.GetField("lastName","LastName",UIFieldTypes.Text),
                        UIField.GetField("gender","Gender", "genderType","gender","Select Gender",UIFieldTypes.DropDown),
                        UIField.GetField("email","Email",UIFieldTypes.Text),
                        UIField.GetField("secondaryEmail","Secondary Email",UIFieldTypes.Text),
                        UIField.GetField("phoneNo","Phone No",UIFieldTypes.Text),
                        UIField.GetField("officePhone","Office Phone",UIFieldTypes.Text),
                        UIField.GetField("website","Website",UIFieldTypes.Text),
                        UIField.GetField("skype","Skype",UIFieldTypes.Text),
                        UIField.GetField("facebook","Facebook",UIFieldTypes.Text),
                        UIField.GetField("twitter","Twitter",UIFieldTypes.Text),
                        UIField.GetField("googlePlus","Google Plus",UIFieldTypes.Text),
                        UIField.GetField("linkedIn","Linked In",UIFieldTypes.Text),
                        UIField.GetField("organization","Organization",UIFieldTypes.Text),
                        UIField.GetField("designation","Designation",UIFieldTypes.Text),
                        UIField.GetField("city","City",UIFieldTypes.Text),
                        UIField.GetField("country","Country",UIFieldTypes.Text),
                        UIField.GetField("address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("communicationAddress", "Communication Address", UIFieldTypes.TextArea),
                        UIField.GetField("dateOfBirth", "Date Of Birth", UIFieldTypes.DateTime),
                        UIField.GetField("expertise","Expertise",UIFieldTypes.Text),
                        UIField.GetField("comments", "Comments", UIFieldTypes.TextArea),
                    };
                    break;
            }

            return PartialView("_FormTemplate", fields);
        }
    }
}