using NetBlog.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public ICollection<CommentView> Comments { get; set; }
    }

    public class PostsView
    {
        public ICollection<PostView> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class PostInput
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
