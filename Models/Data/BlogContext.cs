using Microsoft.EntityFrameworkCore;

namespace NetBlog.Models.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options)
    : base(options)
        {
        }
    }
}
