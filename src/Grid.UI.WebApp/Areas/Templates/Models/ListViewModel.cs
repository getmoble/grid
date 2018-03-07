using System.Collections.Generic;

namespace Grid.UI.WebApp.Areas.Templates.Models
{
    public class ListViewModel
    {
        public string Title { get; set; }
        public bool CanManage { get; set; }
        public IList<UIField> Fields { get; set; }

        public bool ShowDetailsLink { get; set; }

        public bool ShowCreateLink { get; set; }
        public string CreateLink { get; set; }

        public ListViewModel()
        {
            ShowDetailsLink = true;
            CreateLink = "#";
            Fields = new List<UIField>();
        }
    }
}