using FootyBlog.Application.Interfaces;
using FootyBlog.Application.Services;
using FootyBlog.Domain.Entities;
using FootyBlog.Infrastructure.Identity;
using FootyBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();


builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddUserStore<UserStore>()
    .AddRoleStore<RoleStore>()
    .AddSignInManager();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
});

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ILikeRepository, LikeRepository>();

builder.Services.AddScoped<ILikeService, LikeService>();

builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();


var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
