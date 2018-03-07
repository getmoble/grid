using System.Collections.Generic;

namespace Grid.Areas.PMS.Models
{
    public class EstimateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comments { get; set; }

        public List<EstimateLineItemModel> Rows { get; set; }
    }
}