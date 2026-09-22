using Microsoft.AspNetCore.Identity;

namespace FootyBlog.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? RoleId { get; set; }
    }
}
