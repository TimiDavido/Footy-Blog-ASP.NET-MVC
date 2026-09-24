using FootyBlog.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FootyBlog.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(RegisterDto dto);
        Task<bool> UsernameExists(string UserName);
    }
}
