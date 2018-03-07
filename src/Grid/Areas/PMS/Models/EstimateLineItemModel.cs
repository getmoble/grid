namespace Grid.Areas.PMS.Models
{
    public class EstimateLineItemModel
    {
        public string Module { get; set; }
        public string Task { get; set; }
        public double Effort { get; set; }
        public string Comment { get; set; }
        public int WorkType { get; set; }
    }
}