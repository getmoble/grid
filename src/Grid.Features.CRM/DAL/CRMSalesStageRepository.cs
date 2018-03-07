using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMSalesStageRepository : GenericRepository<CRMSalesStage>, ICRMSalesStageRepository
    {
        public CRMSalesStageRepository(IDbContext context) : base(context)
        {

        }
    }
}