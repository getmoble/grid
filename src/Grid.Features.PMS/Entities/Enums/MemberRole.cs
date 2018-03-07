using System.ComponentModel;

namespace Grid.Features.PMS.Entities.Enums
{
    public enum MemberRole
    {
        [Description("Developer")]
        Developer,
        [Description("Lead")]
        Lead,
        [Description("Tester")]
        Tester,
        [Description("Project Manager")]
        ProjectManager,
        [Description("Sales")]
        Sales,
        [Description("Designer")]
        Designer
    }
}
