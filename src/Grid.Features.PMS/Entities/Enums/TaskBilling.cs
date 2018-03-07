using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Features.PMS.Entities.Enums
{
    public enum TaskBilling
    {
        [Description("Non-Billable")]
        NonBillable,
        [Description("Billable")]
        Billable

    }
}
