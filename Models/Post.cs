using Microsoft.EntityFrameworkCore;
using NetBlog.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class PostViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
