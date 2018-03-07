using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
    {
        public UserSkillRepository(IDbContext context) : base(context)
        {

        }
    }
}