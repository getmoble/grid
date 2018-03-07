using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Grid.Data;
using Grid.Entities.HRMS.Enums;
using Grid.Entities.IT.Enums;
using Grid.Entities.LMS.Enums;
using Grid.Entities.Payroll.Enums;
using Grid.Entities.PMS.Enums;
using Grid.Entities.Recruit.Enums;
using Grid.Entities.RMS.Enums;
using Grid.UI.WebApp.Areas.Templates.Models;

namespace Grid.UI.WebApp.Areas.Templates.Controllers
{
    public class FormController : Controller
    {
        private readonly GridDataContext _gridDataContext;

        public FormController(GridDataContext gridDataContext)
        {
            _gridDataContext = gridDataContext;
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = e.ToString() })).ToList();
        }

        public ActionResult Index(string type)
        {
            var viewModel = new FormViewModel();
            switch (type)
            {
                case "Locations":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Phone"),
                        UIField.GetField("Address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
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

                    viewModel.Fields = new List<UIField>
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
                case "EmailTemplates":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Content", "Content", UIFieldTypes.HtmlEditor)
                    };
                    break;

                case "Technologies":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("Icon"),
                        UIField.GetField("Category")
                    };
                    break;

                case "RequirementCategories":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title")
                    };
                    break;
                case "Requirements":
                    ViewBag.AssignedToUserId = new SelectList(_gridDataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name");
                    ViewBag.ContactId = new SelectList(_gridDataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name");
                    ViewBag.Source = new SelectList(_gridDataContext.CRMLeadSources, "Id", "Title");
                    ViewBag.CategoryId = new SelectList(_gridDataContext.RequirementCategories, "Id", "Title");
                    ViewData["TechnologyIds"] = new MultiSelectList(_gridDataContext.Technologies, "Id", "Title");
                    ViewBag.BillingType = GetEnumSelectList<BillingType>();

                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("AssignedToUserId", "Assigned To User", UIFieldTypes.DropDown),
                        UIField.GetField("ContactId", "Contact", UIFieldTypes.DropDown),
                        UIField.GetField("Source", "Source", UIFieldTypes.DropDown),
                        UIField.GetField("CategoryId", "Category", UIFieldTypes.DropDown),
                        UIField.GetField("TechnologyIds", "Technologies", UIFieldTypes.MultiSelectDropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("Url"),
                        UIField.GetField("BillingType", "Billing Type", UIFieldTypes.DropDown),
                        UIField.GetField("Budget"),
                        UIField.GetField("PostedOn", "Posted On", UIFieldTypes.DateTime),
                    };
                    break;
                case "SoftwareCategories":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                    };
                    break;
                case "Softwares":
                    ViewBag.Status = GetEnumSelectList<SoftwareStatus>();
                    ViewBag.LicenseType = GetEnumSelectList<LicenseType>();
                    ViewBag.SoftwareCategoryId = new SelectList(_gridDataContext.SoftwareCategories, "Id", "Title");
                    viewModel.Fields = new List<UIField>
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
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Holidays":
                    ViewBag.HolidayType = GetEnumSelectList<HolidayType>();
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("HolidayType", "Holiday Type", UIFieldTypes.DropDown),
                        UIField.GetField("Date", "Date", UIFieldTypes.DateTime)
                    };
                    break;
                case "LeaveTypes":
                    viewModel.Fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "LeavePeriods":
                    viewModel.Fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                         UIField.GetField("Start", "Start Date", UIFieldTypes.DateTime),
                         UIField.GetField("End", "End Date", UIFieldTypes.DateTime)
                    };
                    break;
                case "Leaves":
                    ViewBag.LeaveTypeId = new SelectList(_gridDataContext.LeaveTypes, "Id", "Title");
                    ViewBag.Duration = GetEnumSelectList<LeaveDuration>();
                    return PartialView("_Leave");
                case "Departments":
                    ViewBag.ParentId = new SelectList(_gridDataContext.Departments, "Id", "Title");
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("MailAlias", "Mail Alias"),
                        UIField.GetField("ParentId", "Parent", UIFieldTypes.DropDown),
                    };
                    break;
                case "Designations":
                    ViewBag.DepartmentId = new SelectList(_gridDataContext.Designations, "Id", "Title");
                    ViewBag.Band = GetEnumSelectList<Band>();
                    viewModel.Fields = new List<UIField>
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
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("StartTime", "Start Time", UIFieldTypes.DateTime),
                        UIField.GetField("EndTime", "End Time", UIFieldTypes.DateTime),
                        UIField.GetField("NeedsCompensation", "Needs Compensation", UIFieldTypes.CheckBox)
                    };
                    break;
                
                case "Roles":
                    ViewData["PermissionIds"] = new MultiSelectList(_gridDataContext.Permissions, "Id", "Title");
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("PermissionIds", "Permissions", UIFieldTypes.MultiSelectDropDown),
                    };
                    break;

                case "Skills":
                    viewModel.Fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Hobbies":
                    viewModel.Fields = new List<UIField>
                    {
                         UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "Certifications":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "JobOpenings":
                    ViewBag.OpeningStatus = GetEnumSelectList<JobOpeningStatus>();
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("NoOfVacancies", "No Of Vacancies"),
                        UIField.GetField("OpeningStatus", "Opening Status", UIFieldTypes.DropDown),
                    };
                    break;
                case "Candidates":
                    ViewBag.Status = GetEnumSelectList<CandidateStatus>();
                    ViewBag.Source = GetEnumSelectList<CandidatesSource>();
                    ViewData["TechnologyIds"] = new MultiSelectList(_gridDataContext.Technologies, "Id", "Title");
                    ViewBag.Gender = GetEnumSelectList<Gender>();
                    ViewBag.CandidateDesignation = new SelectList(_gridDataContext.CandidateDesignations, "Id", "Title");

                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("RecievedOn", "Recieved On", UIFieldTypes.DateTime),
                        UIField.GetField("Status", "Status", UIFieldTypes.DropDown),
                        UIField.GetField("Source", "Source", UIFieldTypes.DropDown),
                        UIField.GetField("TechnologyIds", "Technologies", UIFieldTypes.MultiSelectDropDown),
                        UIField.GetField("FirstName", "First Name", UIFieldTypes.Text),
                        UIField.GetField("MiddleName", "Middle Name", UIFieldTypes.Text),
                        UIField.GetField("LastName", "Last Name", UIFieldTypes.Text),
                        UIField.GetField("Gender", "Gender", UIFieldTypes.DropDown),
                        UIField.GetField("Email", "Email", UIFieldTypes.Text),
                        UIField.GetField("SecondaryEmail", "Secondary Email", UIFieldTypes.Text),
                        UIField.GetField("PhoneNo", "Phone Number", UIFieldTypes.Text),
                        UIField.GetField("OfficePhone", "Office Phone", UIFieldTypes.Text),
                        UIField.GetField("Skype", "Skype", UIFieldTypes.Text),
                        UIField.GetField("Facebook", "Facebook", UIFieldTypes.Text),
                        UIField.GetField("Twitter", "Twitter", UIFieldTypes.Text),
                        UIField.GetField("GooglePlus", "Google Plus", UIFieldTypes.Text),
                        UIField.GetField("LinkedIn", "LinkedIn", UIFieldTypes.Text),
                        UIField.GetField("Organization", "Organization", UIFieldTypes.Text),
                        UIField.GetField("Designation", "Designation", UIFieldTypes.Text),
                        UIField.GetField("CandidateDesignation", "Candidate Designation", UIFieldTypes.DropDown),
                        UIField.GetField("Address", "Address", UIFieldTypes.TextArea),
                        UIField.GetField("CommunicationAddress", "Communication Address", UIFieldTypes.TextArea),
                        UIField.GetField("DateOfBirth", "Date Of Birth", UIFieldTypes.Text),
                        UIField.GetField("Qualification", "Qualification", UIFieldTypes.Text),
                        UIField.GetField("TotalExperience", "Total Experience", UIFieldTypes.Text),
                        UIField.GetField("CurrentCTC", "Current CTC", UIFieldTypes.Text),
                        UIField.GetField("ExpectedCTC", "Expected CTC", UIFieldTypes.Text),
                        UIField.GetField("Comments", "Comments", UIFieldTypes.TextArea),
                    };
                    break;
                case "LeadSources":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "LeadCategories":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "LeadStatus":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name")
                    };
                    break;
                case "Projects":
                    ViewBag.ParentId = new SelectList(_gridDataContext.Projects, "Id", "Title");
                    ViewBag.ClientId = new SelectList(_gridDataContext.CRMContacts.Include("Person"), "Id", "Person.Name");
                    ViewData["TechnologyIds"] = new MultiSelectList(_gridDataContext.Technologies, "Id", "Title");
                    ViewBag.Status = GetEnumSelectList<ProjectStatus>();
                    viewModel.Fields = new List<UIField>
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
                case "Tasks":
                    ViewBag.ProjectId = new SelectList(_gridDataContext.Projects, "Id", "Title");
                    ViewBag.AssigneeId = new SelectList(_gridDataContext.Users.Include("Person"), "Id", "Person.Name");
                    ViewBag.TaskStatus = GetEnumSelectList<ProjectTaskStatus>();
                    ViewBag.Priority = GetEnumSelectList<TaskPriority>();
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("ProjectId", "Project", UIFieldTypes.DropDown),
                        UIField.GetField("AssigneeId", "Assignee", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("ExpectedTime", "Expected Time"),
                        UIField.GetField("TaskStatus", "Status", UIFieldTypes.DropDown),
                        UIField.GetField("Priority", "Priority", UIFieldTypes.DropDown),
                        UIField.GetField("StartDate", "Start Date", UIFieldTypes.DateTime),
                        UIField.GetField("EndDate", "End Date", UIFieldTypes.DateTime)
                    };
                    break;
                case "ArticleCategories":
                    ViewBag.ParentCategoryId = new SelectList(_gridDataContext.Categories, "Id", "Title");
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea),
                        UIField.GetField("IsPublic", "Is Public", UIFieldTypes.CheckBox),
                        UIField.GetField("ParentCategoryId", "Parent Category", UIFieldTypes.DropDown)
                    };
                    break;
                case "Articles":
                    ViewBag.CategoryId = new SelectList(_gridDataContext.Categories, "Id", "Title");
                    viewModel.Fields = new List<UIField>
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
                case "TicketCategories":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };
                    break;
                case "TicketSubCategories":
                    ViewBag.TicketCategoryId = new SelectList(_gridDataContext.TicketCategories, "Id", "Title");
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("TicketCategoryId", "Ticket Category", UIFieldTypes.DropDown),
                        UIField.GetField("Title"),
                        UIField.GetField("Description", "Description", UIFieldTypes.TextArea)
                    };

                    break;
                case "Tickets":
                    return PartialView("_Ticket");
            }

            return PartialView("_Bootstrap", viewModel);
        }
    }
}