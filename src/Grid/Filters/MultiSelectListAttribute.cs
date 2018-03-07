using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Grid.Data;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Infrastructure;

namespace Grid.Filters
{
    public enum MultiSelectListState
    {
        Create,
        Recreate
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MultiSelectListAttribute: FilterAttribute, IResultFilter
    {
        private readonly string _entity;
        private readonly string _property;
        private readonly MultiSelectListState _state;

        private ResultExecutingContext _filterContext;

        public GridDataContext DataContext { get; set; }    

        public MultiSelectListAttribute(string entity, string property, MultiSelectListState state = MultiSelectListState.Create)
        {
            _entity = entity;
            _property = property;
            _state = state;
        }

        private MultiSelectList GenerateSelectList(IEnumerable items, string dataValue, string dataText)
        {
            if (_state == MultiSelectListState.Create)
            {
                return new MultiSelectList(items, dataValue, dataText);
            }

            var formCollection = new FormCollection(_filterContext.HttpContext.Request.Form);
            var selectedValue = formCollection.Get(_property);

            return selectedValue != null ? new MultiSelectList(items, dataValue, dataText, selectedValue) : new MultiSelectList(items, dataValue, dataText);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _filterContext = filterContext;

            switch (_entity)
            {
                case "Role":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Roles, "Id", "Name"));
                    break;
                case "Project":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Projects, "Id", "Title"));
                    break;
                case "Technology":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Technologies, "Id", "Title"));
                    break;
                case "User":
                    SetViewBag(filterContext, GenerateSelectList(DataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name"));
                    break;
                case "LoggedInUser":
                    var gridUser = filterContext.HttpContext.User as Principal;
                    if (gridUser != null)
                    {
                        SetViewBag(filterContext, new MultiSelectList(DataContext.Users.Include("Person").Where(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex), "Id", "Person.Name", new[] { gridUser.Id }));
                    }
                    break;
            }
            
        }

        private void SetViewBag(ResultExecutingContext filterContext, object obj)
        {
            if (_state == MultiSelectListState.Create)
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
