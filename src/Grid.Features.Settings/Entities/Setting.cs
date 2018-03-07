using Grid.Features.Common;

namespace Grid.Features.Settings.Entities
{
    public class Setting : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
    }
}
