using System.ComponentModel;

namespace Swift.UI.ViewModels.Asset
{
    public class AssetViewModel : ViewModelBase
    {
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
