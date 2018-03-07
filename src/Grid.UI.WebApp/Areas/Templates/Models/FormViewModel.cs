using System.Collections.Generic;

namespace Grid.UI.WebApp.Areas.Templates.Models
{
    public class FormViewModel
    {
        public string Title { get; set; }
        public IList<UIField> Fields { get; set; }

        public FormViewModel()
        {
            Fields = new List<UIField>();
        }
    }
}