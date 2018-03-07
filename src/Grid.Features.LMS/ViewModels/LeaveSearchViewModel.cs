using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.LMS.Entities.Enums;

namespace Grid.Features.LMS.ViewModels
{
    public class LeaveSearchViewModel: ViewModelBase
    {
        // If mine is set, we will get only related to current logged in user.
        public bool? Mine { get; set; }

        // If team is set, we will get the team related to current logged in user.
        public bool? Team { get; set; }

        public int? LeaveTypeId { get; set; }
        public int? SubmittedUserById { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }
        public List<LeaveViewModel> Leaves { get; set; }

        public LeaveStatus? Status { get; set; }

        public bool HasTeam { get; set; }

        public LeaveSearchViewModel()
        {
            Leaves = new List<LeaveViewModel>();
        }
    }
}