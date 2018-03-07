using System.ComponentModel;

namespace Grid.Features.Recruit.Entities.Enums
{
    public enum CandidatesSource
    {
        [Description("Agency")]
        Agency,
        [Description("Referal")]
        Referal,
        [Description("Website")]
        Website,
        [Description("Unknown")]
        Unknown,
        [Description("Job Website")]
        JobWebsite
    }
}