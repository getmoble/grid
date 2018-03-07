using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.CRM.ViewModels
{
    public class CRMActivityViewModel: ViewModelBase
    {
        public int? StatusId { get; set; }

        public int CRMLeadId { get; set; }
        public int CRMPotentialId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }
    }
}