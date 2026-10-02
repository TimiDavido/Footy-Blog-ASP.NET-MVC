using FootyBlog.Application.DTOs;
using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    public AccountService(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }
    public async Task<IdentityResult> Register(RegisterDto dto)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result =  await _userManager.CreateAsync(user, dto.Password);


        if(result.Succeeded)
        {
            var emailRequest = new SendSingleMailRequest
            {
                Receiver = dto.Email,
                Subject = "Welcome to FootyBlog!",
                Body = $"Hello {dto.UserName},\n\nThank you for registering at FootyBlog. We're excited to have you on board!"
            };

            await _emailService.SendEmail(emailRequest);
        }

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