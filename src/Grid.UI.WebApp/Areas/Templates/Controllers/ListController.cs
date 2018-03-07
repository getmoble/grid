using System.Collections.Generic;
using System.Web.Mvc;
using Grid.Infrastructure;
using Grid.UI.WebApp.Areas.Templates.Models;

namespace Grid.UI.WebApp.Areas.Templates.Controllers
{
    public class ListController : GridBaseController
    {
        public ActionResult Index(string type, string mode = "Manage")
        {
            var viewModel = new ListViewModel();
            switch (type)
            {
                case "Locations":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#location/create";
                    viewModel.Title = "Manage Locations";
                    viewModel.CanManage = WebUser.Permissions.Contains(200);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Address"),
                        UIField.GetField("Phone"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Permissions":
                    viewModel.ShowDetailsLink = false;
                    viewModel.Title = "Manage Permissions";
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("PermissionCode", "Permission Code"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Roles":
                    viewModel.Title = "Manage Roles";
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#role/create";
                    viewModel.CanManage = WebUser.Permissions.Contains(200);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Technologies":
                    viewModel.Title = "Manage Technologies";
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#technology/create";
                    viewModel.CanManage = WebUser.Permissions.Contains(200);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Icon"),
                        UIField.GetField("Category"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Awards":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#award/create";
                    viewModel.Title = "Manage Awards";
                    viewModel.CanManage = WebUser.Permissions.Contains(200);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "EmailTemplates":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#emailtemplate/create";
                    viewModel.Title = "Manage Email Templates";
                    viewModel.CanManage = WebUser.Permissions.Contains(200);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Users":
                    viewModel.Title = "Employees";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    if (viewModel.CanManage)
                    {
                        viewModel.ShowCreateLink = true;
                        viewModel.CreateLink = "#user/create";
                        viewModel.Title = "Manage Employees";
                    }
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Code"),
                        UIField.GetField("Location"),
                        UIField.GetField("Department"),
                        UIField.GetField("Designation"),
                        UIField.GetField("LastActivity", "Last Activity", UIFieldTypes.DateTime)
                    };
                    break;
                case "Candidates":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#candidate/create";
                    viewModel.Title = "Manage Candidates";
                    viewModel.CanManage = WebUser.Permissions.Contains(500);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Code"),
                        UIField.GetField("TotalExperience", "Total Experience"),
                        UIField.GetField("Source"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Interviews":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("ScheduledOn"),
                        UIField.GetField("Status"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                
               case "TeamLeaves":
                    viewModel.Fields = new List<UIField>
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
                case "Departments":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#department/create";
                    viewModel.Title = "Manage Departments";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("MailAlias", "Mail Alias"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Designations":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#designation/create";
                    viewModel.Title = "Manage Designations";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Department"),
                        UIField.GetField("Band"),
                        UIField.GetField("MailAlias"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Shifts":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#shift/create";
                    viewModel.Title = "Manage Shifts";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("StartTime", "Start Time", UIFieldTypes.DateTime),
                        UIField.GetField("EndTime", "End Time", UIFieldTypes.DateTime),
                        UIField.GetField("NeedsCompensation", "Needs Compensation", UIFieldTypes.DateTime),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Skills":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#skill/create";
                    viewModel.Title = "Manage Skills";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Hobbies":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#hobby/create";
                    viewModel.Title = "Manage Hobbies";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Certifications":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#certification/create";
                    viewModel.Title = "Manage Certifications";
                    viewModel.CanManage = WebUser.Permissions.Contains(210);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "JobOpenings":
                    viewModel.Title = "Job Openings";
                    viewModel.CanManage = WebUser.Permissions.Contains(500);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Vacancies"),
                        UIField.GetField("Status"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "CandidateDesignations":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Rounds":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Offers":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Holidays":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#holiday/create";
                    viewModel.Title = "Manage Holidays";
                    viewModel.CanManage = WebUser.Permissions.Contains(215);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Type"),
                        UIField.GetField("Date", "Date", UIFieldTypes.DateTime),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "LeaveTypes":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leavetype/create";
                    viewModel.Title = "Manage Leave Types";
                    viewModel.CanManage = WebUser.Permissions.Contains(215);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "LeavePeriods":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leaveperiod/create";
                    viewModel.Title = "Manage Leave Periods";
                    viewModel.CanManage = WebUser.Permissions.Contains(215);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Start", "Start", UIFieldTypes.DateTime),
                        UIField.GetField("End", "End", UIFieldTypes.DateTime),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Leaves":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leave/create";
                    viewModel.Title = "Manage Leaves";
                    viewModel.CanManage = WebUser.Permissions.Contains(215);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Employee"),
                        UIField.GetField("LeaveType", "Leave Type"),
                        UIField.GetField("Duration"),
                        UIField.GetField("Period"),
                        UIField.GetField("AppliedOn", "Applied On", UIFieldTypes.DateTime),
                        UIField.GetField("Status"),
                        UIField.GetField("Approver"),
                        UIField.GetField("ActedOn", "Acted On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Vendors":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Email"),
                        UIField.GetField("Phone"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "AssetCategories":
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "SoftwareCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#softwarecategory/create";
                    viewModel.Title = "Manage Software Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(230);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Softwares":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#software/create";
                    viewModel.Title = "Manage Softwares";
                    viewModel.CanManage = WebUser.Permissions.Contains(230);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Version"),
                        UIField.GetField("Status"),
                    };
                    break;
                case "LeadSources":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leadsource/create";
                    viewModel.Title = "Manage Lead Sources";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "LeadStatus":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leadstatus/create";
                    viewModel.Title = "Manage Lead Statues";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name")
                    };
                    break;
                case "LeadCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#leadcategory/create";
                    viewModel.Title = "Manage Lead Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description")
                    };
                    break;
                case "Leads":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#lead/create";
                    viewModel.Title = "Manage Leads";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Expertise"),
                        UIField.GetField("Description")
                    };
                    break;
                case "SalesStages":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#salesstage/create";
                    viewModel.Title = "Manage Sales Stages";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Name"),
                        UIField.GetField("Status")
                    };
                    break;
                case "PotentialCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#potentialcategory/create";
                    viewModel.Title = "Manage Potential Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Description")
                    };
                    break;
                case "Potentials":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#potential/create";
                    viewModel.Title = "Manage Potentials";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("ExpectedAmount"),
                        UIField.GetField("ExpectedCloseDate"),
                        UIField.GetField("Description")
                    };
                    break;
                case "CRMAccounts":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#crmaccount/create";
                    viewModel.Title = "Manage Accounts";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Industry"),
                        UIField.GetField("Email"),
                        UIField.GetField("PhoneNo"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Contacts":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#contact/create";
                    viewModel.Title = "Manage Contacts";
                    viewModel.CanManage = WebUser.Permissions.Contains(220);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Expertise"),
                        UIField.GetField("Comments"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "RequirementCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#requirementcategory/create";
                    viewModel.Title = "Manage Requirement Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(225);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Requirements":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#requirement/create";
                    viewModel.Title = "Manage Requirements";
                    viewModel.CanManage = WebUser.Permissions.Contains(225);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Source"),
                        UIField.GetField("Category"),
                        UIField.GetField("BillingType", "Billing Type"),
                        UIField.GetField("Budget"),
                        UIField.GetField("Status"),
                        UIField.GetField("PostedOn", "Posted On", UIFieldTypes.DateTime),
                        UIField.GetField("RespondedOn", "Responded On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Projects":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#project/create";
                    viewModel.Title = "Manage Projects";
                    viewModel.CanManage = WebUser.Permissions.Contains(240);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("Status", "Status"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Tasks":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#task/create";
                    viewModel.Title = "Manage Tasks";
                    viewModel.CanManage = WebUser.Permissions.Contains(240);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Code"),
                        UIField.GetField("Title"),
                        UIField.GetField("Project.Title", "Project"),
                        UIField.GetField("Priority"),
                        UIField.GetField("StartDate", "Start Date", UIFieldTypes.DateTime),
                        UIField.GetField("DueDate", "Due Date", UIFieldTypes.DateTime),
                        UIField.GetField("TaskStatus", "Status"),
                        UIField.GetField("CreatedOn", "Applied On", UIFieldTypes.DateTime)
                    };
                    break;
                case "ArticleCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#articlecategory/create";
                    viewModel.Title = "Manage Article Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(700);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("IsPublic"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Articles":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#article/create";
                    viewModel.Title = "Manage Articles";
                    viewModel.CanManage = WebUser.Permissions.Contains(700);
                    viewModel.Fields = new List<UIField>
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
                case "TicketCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#ticketcategory/create";
                    viewModel.Title = "Manage Ticket Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(1100);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "TicketSubCategories":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#ticketsubcategory/create";
                    viewModel.Title = "Manage Ticket Sub Categories";
                    viewModel.CanManage = WebUser.Permissions.Contains(1100);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("CreatedOn", "Created On", UIFieldTypes.DateTime)
                    };
                    break;
                case "Tickets":
                    viewModel.ShowCreateLink = true;
                    viewModel.CreateLink = "#ticket/create";
                    viewModel.Title = "Manage Tickets";
                    viewModel.CanManage = WebUser.Permissions.Contains(1100);
                    viewModel.Fields = new List<UIField>
                    {
                        UIField.GetField("Title"),
                        UIField.GetField("DueDate", "Due Date", UIFieldTypes.DateTime),
                        UIField.GetField("AssignedTo", "Assigned To"),
                        UIField.GetField("Status"),
                        UIField.GetField("CreatedBy", "Created By"),
                        UIField.GetField("LastUpdatedOn", "Last Updated On", UIFieldTypes.DateTime)
                    };
                    break;
            }

            return PartialView("_Table", viewModel);
        }
    }
}