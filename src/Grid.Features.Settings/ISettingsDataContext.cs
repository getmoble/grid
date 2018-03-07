using System.Data.Entity;
using Grid.Features.Common;
using Grid.Features.Settings.Entities;

namespace Grid.Features.Settings
{
    public interface ISettingsDataContext : IDbContext
    {
        DbSet<Setting> Settings { get; set; }
    }
}
