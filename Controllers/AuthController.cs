using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AspnetCoreMvcFull.Controllers;

public class AuthController : Controller
{
  private readonly ILoginRepository _loginRepository;
 
  public AuthController(ILoginRepository loginRepository)
  {
    _loginRepository = loginRepository;
  }
  public IActionResult ForgotPasswordBasic() => View();
  public IActionResult LoginBasic()
  {
    return View();
    }
  [HttpPost]

  public async Task<IActionResult> LoginBasicAsync(LoginModel login)
  {
    try
    {
      // Perform login and get the details
      LoginModelDetails loginModelDetails = _loginRepository.Login(login);

      // Check if login is successful
      if (loginModelDetails != null && loginModelDetails.Data.Count > 0 && loginModelDetails.Data[0].Status == "Login successful")
      {
        // Create claims
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginModelDetails.Data[0].VCH_USERNAME.ToString()),
                new Claim(ClaimTypes.Name, loginModelDetails.Data[0].VCH_USERNAME.ToString()),
                new Claim(ClaimTypes.Role,"Admin"),
            };

        // Create ClaimsIdentity and ClaimsPrincipal
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign in the user
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        var nameValue = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        // Redirect to the dashboard
        return RedirectToAction("Index", "Dashboards");
      }
      else
      {
        // Handle unsuccessful login
        // You may want to log or handle the failure appropriately
        return View("LoginFailed"); // You can create a specific view for login failure
      }
    }
    catch (Exception ex)
    {
      // Log the exception or handle it as needed
      // ...

      // Rethrow the exception
      throw;
    }
  }


  public async Task<IActionResult> Logout()
  {
    // Sign out the user
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    // Redirect to the home page or any other desired page after logout
    return RedirectToAction("LoginBasic", "Auth");
  }


  public IActionResult RegisterBasic() => View();
}
