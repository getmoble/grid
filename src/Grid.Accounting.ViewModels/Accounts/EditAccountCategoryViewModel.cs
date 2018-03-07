using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Swift.UI.ViewModels.Accounts
{
    public class EditAccountCategoryViewModel : ViewModelBase
    {
        [DisplayName("Name *")]
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        public IList<SelectListItem> AccountCategory { get; set; }
        [DisplayName("Account Category")]
        public string SelectedAccountCategory { get; set; }
        [DisplayName("Remarks")]

        public string Remarks { get; set; }
    }
}
