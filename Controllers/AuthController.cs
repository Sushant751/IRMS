using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Repository;

namespace AspnetCoreMvcFull.Controllers
{
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
    [ValidateAntiForgeryToken] // Adding Anti-Forgery Token for security
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
                        new Claim(ClaimTypes.NameIdentifier, loginModelDetails.Data[0].VCH_USERNAME),
                        new Claim(ClaimTypes.Name, loginModelDetails.Data[0].VCH_USERNAME),
                        new Claim(ClaimTypes.Role, loginModelDetails.Data[0].Role),
                    };

          // Create ClaimsIdentity and ClaimsPrincipal
          var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          var principal = new ClaimsPrincipal(identity);

          // Sign in the user
          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

          // Redirect to the dashboard
          return RedirectToAction("Index", "Dashboards");
        }
        else
        {
          // Handle unsuccessful login
          // You may want to log or handle the failure appropriately
          ModelState.AddModelError(string.Empty, "Invalid username or password");
          return View("LoginBasic", login); // Return to login page with an error message
        }
      }
      catch (Exception ex)
      {
        // Log the exception or handle it as needed
        // Log.Error(ex, "Login error");
        ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        return View("LoginBasic", login); // Return to login page with an error message
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
}
