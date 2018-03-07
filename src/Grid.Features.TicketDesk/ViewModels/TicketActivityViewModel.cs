using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class TicketActivityViewModel : ViewModelBase
    {
        public int TicketId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }
    }
}