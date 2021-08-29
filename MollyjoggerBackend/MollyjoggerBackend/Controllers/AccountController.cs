using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.Areas.AdminPanel.Utils;
using MollyjoggerBackend.Data;
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

        #region AccountLogin
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

            if (existUser.IsActive == false)
            {
                ModelState.AddModelError("", "Your account is disabled.");
                return View();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(existUser, login.Password,login.RememberMe,true);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is invalid");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region AccountRegister
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
                FullName=register.Fullname,
                Email=register.Email,
                IsActive=true

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
            await _userManager.AddToRoleAsync(newUser, RoleConstants.MemberRole);
            await _signInManager.SignInAsync(newUser, true);
            
            return RedirectToAction("Index","Home");
        }
        #endregion

        #region AccountLogout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion


        public IActionResult RedirectionToResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RedirectionToResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var dbUser = await _userManager.FindByEmailAsync(email);

            if (dbUser == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);

            var link = Url.Action("ResetPassword", "Account", new { dbUser.Id, token }, protocol: HttpContext.Request.Scheme);
            var message = $"<a href={link}>For Reset password click here</a>";
            await EmailUtil.SendEmailAsync(dbUser.Email, message, "ResetPassword");

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> ResetPassword(string id, string token)
        {

            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);

            if (dbUser == null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token, ResetPasswordViewModel passwordViewModel)
        {

            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(passwordViewModel);
            }

            var dbUser = await _userManager.FindByIdAsync(id);

            if (id == null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(dbUser, token, passwordViewModel.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(passwordViewModel);
            }

            return RedirectToAction("Login");
        }
    }
}
