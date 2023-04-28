using MainService.Interfaces;
using MainService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainService.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class LoginModel : PageModel
{
    [BindProperty]
    public AuthModel Auth { get; set; }

    [BindProperty] 
    public string RadioButton { get; set; }

    private readonly IAuthService _authService;

    public LoginModel(IAuthService authService)
    {
        _authService = authService;
    }


  
    public async Task<IActionResult> OnPostAsync()
    {
        var result = RadioButton == "login" 
            ? await _authService.Login(Auth.UserLogin, Auth.Password)
            : await _authService.Register(Auth.UserLogin, Auth.Password, Auth.Invite);

        if (result)
        {
            return Redirect("~/");
        }

        return Page();
    }
}
