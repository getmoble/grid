using Grid.Features.CRM.Entities.Enums;
using Grid.Features.HRMS;

namespace Grid.Features.CRM.Entities
{
    public class CRMSalesStage : UserCreatedEntityBase
    {
        public string Name { get; set; }
        public SaleStatus Status { get; set; }
    }
}