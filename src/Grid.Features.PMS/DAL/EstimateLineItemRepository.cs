﻿using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class EstimateLineItemRepository : GenericRepository<EstimateLineItem>, IEstimateLineItemRepository
    {
        public EstimateLineItemRepository(IDbContext context) : base(context)
        {

        }
    }
}
