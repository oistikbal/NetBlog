using Microsoft.AspNetCore.Identity;
using NetBlog.Models;

namespace NetBlog.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        public ICollection<Post> Posts { get; }
    }
}
