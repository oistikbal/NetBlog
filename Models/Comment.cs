﻿using NetBlog.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
    }

    public class CommentView
    {
        public string Body { get; set; }
        public string Email { get; set; }
    }

    public class CommentInput
    {
        public int PostId { get; set; }
        public string Body { get; set; }
    }
}
