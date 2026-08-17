using FootyBlog.Models;
using FootyBlog.ServiceContracts;
using FootyBlog.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
