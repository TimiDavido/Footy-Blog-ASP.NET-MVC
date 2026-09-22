using FootyBlog.Application.DTOs;
using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = dto.UserName
        };

        var result =  await _userManager.CreateAsync(user, dto.Password);

        return result;
    }

    public async Task<bool> UsernameExistsAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null && user.UserName == userName)
        { 
            return true;
        }
        return false;
    }
}