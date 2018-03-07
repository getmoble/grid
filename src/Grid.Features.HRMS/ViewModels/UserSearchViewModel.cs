using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using PagedList;

namespace Grid.Features.HRMS.ViewModels
{
    public class UserSearchViewModel: PagedViewModelBase
    {
        public bool ShowExEmployees { get; set; }
        public int? DepartmentId { get; set; }
        public int? LocationId { get; set; }
        public int? DesignationId { get; set; }
        public int? ShiftId { get; set; }
        public EmployeeStatus? Status { get; set; }
        public ViewMode Mode { get; set; }  

        public int Total { get; set; }

        public IPagedList<Features.HRMS.Entities.User> Users { get; set; }
    }
}