using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.Payroll.Entities.Enums;

namespace Grid.Features.Payroll.Entities
{
    public class SalaryComponent: EntityBase
    {
        public string Title { get; set; }
        public Mode Mode { get; set; }
        public double Value { get; set; }

        public int? ParentComponentId { get; set; }
        [ForeignKey("ParentComponentId")]
        public SalaryComponent ParentComponent { get; set; }
    }
}
