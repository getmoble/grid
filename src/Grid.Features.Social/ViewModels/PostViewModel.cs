using System;
using Grid.Features.Social.Entities;

namespace Grid.Features.Social.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByUserImage { get; set; }
        public DateTime CreatedOn { get; set; }

        public PostViewModel()
        {
            
        }

        public PostViewModel(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            Likes = post.Likes;
            Comments = post.Comments;
            CreatedByUserId = post.CreatedByUserId;

            if (post.CreatedByUser?.Person != null)
            {
                CreatedByUserName = post.CreatedByUser.Person.Name;
                CreatedByUserImage = post.CreatedByUser.Person.PhotoPath;
            }
            CreatedOn = post.CreatedOn;
        }
    }
}
