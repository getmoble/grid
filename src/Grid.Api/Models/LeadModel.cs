namespace Grid.Api.Models
{
    public class LeadModel
    {
        public int CreatedByUserId { get; set; }
        public int LeadSourceId { get; set; }
        public int? CategoryId { get; set; }
        public int StatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Message { get; set; }
    }
}