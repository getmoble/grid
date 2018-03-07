
using Grid.Features.HRMS;

namespace Grid.Features.Social.Entities
{
    public class Post : UserCreatedEntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
    }
}
