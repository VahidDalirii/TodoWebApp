using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoWebApp.Models;

namespace ToDoWebApp.Controllers
{
    public class AccountController : Controller
    {
        Helper _helper = new Helper();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login (string username, string password, bool ispremium, string submit)
        {
            Account account = new Account()
            {
                UserName=username,
                Password=password,
                IsPremium=ispremium
            };

            bool accountExists = _helper.CheckIfAccountExists(account);
            if (accountExists && submit=="Login")
            {
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, account.UserName)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else if(!accountExists && submit == "Login")
            {
                TempData["textmsg"] = "<script>alert('You have to register yourself. Your Account is not registred');</script>";
                return RedirectToAction("Login", "Account");
            }
            else if(!accountExists && submit == "Register")
            {
                _helper.CreateAccount(account);

                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, account.UserName)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else if (accountExists && submit=="Register")
            {
                TempData["textmsg"] = "<script>alert('You already have an account registred. You can Login');</script>";
                return RedirectToAction("Login", "Account");
            }

            return Redirect("/Account/Login");
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}