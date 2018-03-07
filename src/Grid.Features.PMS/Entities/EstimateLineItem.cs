using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.PMS.Entities
{
    public class EstimateLineItem : EntityBase
    {
        public int EstimateId { get; set; }
        [ForeignKey("EstimateId")]
        public Estimate Estimate { get; set; }

        public string Module { get; set; }
        public string Task { get; set; }
        public double Effort { get; set; }
        public string Comment { get; set; }
        public int WorkType { get; set; }
    }
}
