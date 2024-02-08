using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlog.Areas.Identity.Data;
using NetBlog.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NetBlogContextConnection") ?? throw new InvalidOperationException("Connection string 'NetBlogContextConnection' not found.");

builder.Services.AddDbContext<NetBlogContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<NetBlogContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapBlazorHub();
app.MapRazorPages();
app.Run();
