using Grid.Features.Common;

namespace Grid.Entities
{
    public class DocumentEntityBase: EntityBase
    {
        public string FileName { get; set; }

        public double FileSize { get; set; }

        public string DocumentPath { get; set; }
    }
}
