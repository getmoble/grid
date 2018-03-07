namespace Grid.Api.Models.TicketDesk
{
    public class TicketSubCategoryModel: ApiModelBase
    {
        public int TicketCategoryId { get; set; }        
        public string TicketCategory { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; }

    }
}
