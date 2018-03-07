using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.ViewModels
{
    public class DesignationSearchViewModel: ViewModelBase
    {
        public int? DepartmentId { get; set; }
        public Band? Band { get; set; }
        public List<Designation> Designations { get; set; }

        public DesignationSearchViewModel()
        {
            Designations = new List<Designation>();
        }
    }
}