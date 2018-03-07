using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class Technology: EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
    }
}