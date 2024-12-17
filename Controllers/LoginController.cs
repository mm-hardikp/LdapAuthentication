using LdapAuthentication.Interface;
using LdapAuthentication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static LdapAuthentication.Services.LdapAuthenticationService;
using LdapAuthentication.Services;
using System.Threading;
using System;

namespace LdapAuthentication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILdapAuthenticationService _authenticationService;

        public LoginController(ILdapAuthenticationService ldapAuthentication)
        {
            _authenticationService = ldapAuthentication;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Authenticate the user using LDAP
                    bool isAuthenticated = _authenticationService.LdapAuthentication(model.UserName, model.Password, cancellationToken);

                    if (isAuthenticated)
                    {
                        // Create claims and sign the user in
                        var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, model.UserName) // Use the LDAP username here
                                };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            new AuthenticationProperties { IsPersistent = model.RememberMe });

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Login error: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            // If we get here, something failed, redisplay the form
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("Logout", "Login");
        }
    }
}

