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
    public async Task<IdentityResult> Register(RegisterDto dto)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result =  await _userManager.CreateAsync(user, dto.Password);

        return result;
    }

    public async Task<bool> UsernameExists(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null && user.UserName == userName)
        { 
            return true;
        }
        return false;
    }

    public async Task<bool> EmailExists(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null && user.Email == email)
        {
            return true;
        }
        return false;
    }
}