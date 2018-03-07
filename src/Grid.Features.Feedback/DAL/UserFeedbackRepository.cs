using Grid.Features.Common;
using Grid.Features.Feedback.DAL.Interfaces;
using Grid.Features.Feedback.Entities;

namespace Grid.Features.Feedback.DAL
{
    public class UserFeedbackRepository : GenericRepository<UserFeedback>, IUserFeedbackRepository
    {
        public UserFeedbackRepository(IDbContext context) : base(context)
        {

        }
    }
}