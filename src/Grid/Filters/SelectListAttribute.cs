using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Infrastructure;
using Grid.Data;

namespace Grid.Filters
{
    public enum SelectListState
    {
        Create,
        Recreate
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SelectListAttribute: FilterAttribute, IResultFilter
    {
        private readonly string _entity;
        private readonly string _property;
        private readonly SelectListState _state;

        private ResultExecutingContext _filterContext;

        public GridDataContext DataContext { get; set; }    

        public SelectListAttribute(string entity, string property, SelectListState state = SelectListState.Create)
        {
            _entity = entity;
            _property = property;
            _state = state;
        }

        private SelectList GenerateSelectList(IEnumerable items, string dataValue, string dataText)
        {
            if (_state == SelectListState.Create)
            {
                return new SelectList(items, dataValue, dataText);
            }

            var formCollection = new FormCollection(_filterContext.HttpContext.Request.Form);
            var selectedValue = formCollection.Get(_property);

            return selectedValue != null ? new SelectList(items, dataValue, dataText, selectedValue) : new SelectList(items, dataValue, dataText);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _filterContext = filterContext;

            // We need to do a caching here, It should be tenant specific, so will do it later.
            switch (_entity)
            {
                case "Department":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Departments, "Id", "Title"));
                    break;
                case "Designation":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Designations, "Id", "Title"));
                    break;
                case "Location":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Locations, "Id", "Title"));
                    break;
                case "Shift":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Shifts, "Id", "Title"));
                    break;
                case "Award":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Awards, "Id", "Title"));
                    break;
                case "AssetCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.AssetCategories, "Id", "Title"));
                    break;
                case "Vendor":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Vendors, "Id", "Title"));
                    break;
                case "Skill":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Skills, "Id", "Title"));
                    break;
                case "Certification":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Certifications, "Id", "Title"));
                    break;
                case "CRMAccount":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMAccounts, "Id", "Title"));
                    break;
                case "CRMContact":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMContacts.Include("Person"), "Id", "Person.Name"));
                    break;
                case "CRMLeadSource":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMLeadSources, "Id", "Title"));
                    break;
                case "CRMLeadCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMLeadCategories, "Id", "Title"));
                    break;
                case "CRMLeadStatus":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMLeadStatuses, "Id", "Name"));
                    break;
                case "CRMPotentialCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMPotentialCategories, "Id", "Title"));
                    break;
                case "CRMSalesStage":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.CRMSalesStages, "Id", "Name"));
                    break;
                case "SoftwareCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.SoftwareCategories, "Id", "Title"));
                    break;
                case "LeaveType":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.LeaveTypes, "Id", "Title"));
                    break;
                case "LeavePeriod":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.LeaveTimePeriods, "Id", "Title"));
                    break;
                case "SalaryComponent":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.SalaryComponents, "Id", "Title"));
                    break;

                case "RequirementCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.RequirementCategories, "Id", "Title"));
                    break;
                case "TicketCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.TicketCategories, "Id", "Title"));
                    break;
                case "TicketSubCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.TicketSubCategories, "Id", "Title"));
                    break;
                case "Project":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Projects, "Id", "Title"));
                    break;
                case "ArticleCategory":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Categories, "Id", "Title"));
                    break;
                case "Permission":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Permissions, "Id", "Title"));
                    break;
                case "Role":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Roles, "Id", "Name"));
                    break;
                case "LoggedInUser":
                    var gridUser = filterContext.HttpContext.User as Principal;
                    if (gridUser != null)
                    {
                        SetViewBag(filterContext, new SelectList(DataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name", gridUser.Id));
                    }
                    break;
                case "User":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name"));
                    break;
                case "Employee":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Employees.Include("User").Include("User.Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "EmployeeId", "User.Person.Name"));
                    break;
            }
            
        }

        private void SetViewBag(ResultExecutingContext filterContext, object obj)
        {
            if (_state == SelectListState.Create)
            {
                if (filterContext != null && obj != null)
                    filterContext.Controller.ViewData.Add(_property, obj);
            }
            else
            {
                if (filterContext != null && obj != null && !_filterContext.Controller.ViewData.ModelState.IsValid)
                    filterContext.Controller.ViewData.Add(_property, obj);
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }
    }
}
