using FootyBlog.Application.DTOs;
using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller

{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAccountService _accountService;
    public AccountController(SignInManager<ApplicationUser> signInManager, IAccountService accountService)
    {
        _signInManager = signInManager;
        _accountService = accountService;
    }

    [Route("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        bool usernameExist = await _accountService.UsernameExists(dto.UserName);

        if (usernameExist)
        {
            ModelState.AddModelError("UserName", "Username already exist");
            return View(dto);
        }

        bool emailExist = await _accountService.EmailExists(dto.Email);

        if (emailExist)
        {
            ModelState.AddModelError("Email", "Email already exist");
            return View(dto);
        }

        var result = await _accountService.Register(dto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Password", error.Description);
            }
            return View(dto);
        }

        TempData["SuccessMessage"] = "User Created Successfully";
        return RedirectToAction("Login");
    }

    [Route("Login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login(RegisterDto dto, string? ReturnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _signInManager.PasswordSignInAsync(
            dto.UserName,
            dto.Password,
            false,
            false);

        if (result.Succeeded)
        {      
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid email or password");

        return View(dto);
    }

    [Route("Logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        
        return RedirectToAction("Index", "Home");
    }
}