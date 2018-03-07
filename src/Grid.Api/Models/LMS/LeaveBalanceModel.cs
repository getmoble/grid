using System;

namespace Grid.Api.Models.LMS
{
    public class LeaveBalanceModel
    {
        public float LeaveCount { get; set; }
        public float Allocation { get; set; }
        public DateTime End { get; set; }

    }
}
