using System.ComponentModel;

namespace Grid.Features.PMS.Entities.Enums
{
    public enum Billing
    {
        [Description("Non-Billable")]
        NonBillable,
        [Description("Billable")]
        Billable
    }
}
