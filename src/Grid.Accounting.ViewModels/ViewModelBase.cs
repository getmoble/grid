using System;
using System.ComponentModel;
using Swift.Entities.Enums;

namespace Swift.UI.ViewModels
{
    public class ViewModelBase
    {
        public long Id { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public OriginType EntityType { get; set; }
        public string EntityConfig { get; set; }
        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }
    }
}
