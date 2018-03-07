using System.ComponentModel.DataAnnotations;

namespace Grid.Api.Models
{
    public class LocationModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Number")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
