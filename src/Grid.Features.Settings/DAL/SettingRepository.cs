using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;

namespace Grid.Features.Settings.DAL
{
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        public SettingRepository(IDbContext context) : base(context)
        {

        }
    }
}