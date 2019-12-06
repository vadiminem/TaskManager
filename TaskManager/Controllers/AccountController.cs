using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Models.Authorization;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {

        private IStorageRepository repository;
        public AccountController(IStorageRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = repository.FindUser(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(user.Username);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            var user = repository.FindUser(model.Email);
            if (user == null)
            {
                repository.InsertUser(
                    new User()
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Username = model.Username
                    });
                await Authenticate(model.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(model);
        }

        private async System.Threading.Tasks.Task Authenticate(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}