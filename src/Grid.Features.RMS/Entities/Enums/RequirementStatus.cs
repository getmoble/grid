using System.ComponentModel;

namespace Grid.Features.RMS.Entities.Enums
{
    public enum RequirementStatus
    {
        [Description("New")]
        New,
        [Description("Reviewing")]
        Reviewing,
        [Description("Need Technical Review")]
        NeedTechnicalReview,
        [Description("Technical Review Completed")]
        TechnicalReviewCompleted,
        [Description("Proposed")]
        Proposed,
        [Description("Hot")]
        Hot,
        [Description("Cold")]
        Cold,
        [Description("Lost")]
        Lost,
        [Description("Won")]
        Won,
        [Description("Rejected")]
        Rejected
    }
}
