using Grid.Features.Common.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grid.Features.User.Entities;

namespace Grid.Features.User.Interfaces.Data
{
    public interface IUserDataContext:IDbContext
    {
        DbSet<Entities.User> Users { get; set; }
    }
}
