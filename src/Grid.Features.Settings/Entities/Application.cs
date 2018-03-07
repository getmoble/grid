using Grid.Features.Common;

namespace Grid.Features.Settings.Entities
{
    public class Application: EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }
}
