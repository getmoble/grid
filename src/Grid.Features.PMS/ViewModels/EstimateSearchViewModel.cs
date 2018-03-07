using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.ViewModels
{
    public class EstimateSearchViewModel: ViewModelBase
    {
        public string Title { get; set; }
        public int? CreatedByUserId { get; set; }

        public List<Estimate> Estimates { get; set; }

        public EstimateSearchViewModel()
        {
            Estimates = new List<Estimate>();
        }
    }
}
