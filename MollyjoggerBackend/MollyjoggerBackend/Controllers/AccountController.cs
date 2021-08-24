using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.Models;
using MollyjoggerBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await _userManager.FindByEmailAsync(login.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Email or password is invalid");
                return View();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(existUser, login.Password, true, true);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is invalid");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var dbUser = await _userManager.FindByNameAsync(register.Username);
            if(dbUser != null)
            {
                ModelState.AddModelError("Username", "There is have already user like this username,please change another");
            }

            var newUser = new User
            {
                UserName=register.Username,
                
                Email=register.Email

            };
            var identityResult = await _userManager.CreateAsync(newUser, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();
            }
            
            
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
