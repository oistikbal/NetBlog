using Bogus;
using Microsoft.AspNetCore.Identity;
using NetBlog.Areas.Identity.Data;

namespace NetBlog.Models.Data
{
	public static class DataSeeder
	{
		public static async Task SeedAsync(IServiceProvider services)
		{
			var userManager = services.GetRequiredService<UserManager<User>>();
			var dbContext = services.GetRequiredService<BlogContext>();

			// Seed users
			var userFaker = new Faker<User>()
				.RuleFor(u => u.UserName, f => f.Internet.UserName())
				.RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName));

			var users = userFaker.Generate(10);
			foreach (var user in users)
			{
				if (await userManager.FindByEmailAsync(user.Email) == null)
				{
					await userManager.CreateAsync(user, "Password123!");
				}
			}
			await dbContext.SaveChangesAsync();

			// Seed posts
			var postFaker = new Faker<Post>()
				.RuleFor(p => p.Title, f => f.Lorem.Sentence())
				.RuleFor(p => p.Body, f => f.Lorem.Paragraph())
				.RuleFor(p => p.UserId, f => userManager.GetUserIdAsync(users[f.Random.Int(0, users.Count - 1)]).Result);

			var posts = postFaker.Generate(50);
			dbContext.Posts.AddRange(posts);
			await dbContext.SaveChangesAsync();

			// Seed comments
			var commentFaker = new Faker<Comment>()
				.RuleFor(c => c.Body, f => f.Lorem.Sentence())
				.RuleFor(c => c.UserId, f => userManager.GetUserIdAsync(users[f.Random.Int(0, users.Count - 1)]).Result)
				.RuleFor(c => c.PostId, f => f.Random.Int(1, 50));

			var comments = commentFaker.Generate(100);
			dbContext.Comments.AddRange(comments);

			await dbContext.SaveChangesAsync();
		}
	}
}
