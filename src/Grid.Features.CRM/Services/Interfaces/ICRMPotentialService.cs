using System;

namespace Grid
    .Features.CRM.Services.Interfaces
{
    public interface ICRMPotentialService
    {
        bool Convert(bool createAccount, bool createPotential, int id, int assignedToUserId, int? categoryId, double? expectedAmount, DateTime? expectedCloseDate, string description, DateTime? enquiredOn, int salesStage, int createdByUserId);
        bool Delete(int id);
    }
}
