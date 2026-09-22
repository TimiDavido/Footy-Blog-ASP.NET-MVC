using FootyBlog.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FootyBlog.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<bool> UsernameExistsAsync(string UserName);
    }
}
