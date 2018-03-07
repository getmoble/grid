using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Auth.Entities;
using Grid.Features.Common;

namespace Grid.Features.Auth.DAL
{
    public class TokenRepository : GenericRepository<Token>, ITokenRepository
    {
        public TokenRepository(IDbContext context) : base(context)
        {

        }
    }
}