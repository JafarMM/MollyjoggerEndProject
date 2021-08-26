using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Data;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
using MollyjoggerBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(AppDbContext dbContext,UserManager<User>userManager,SignInManager<User>signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
          

            var dbUsers = new List<User>();

            if (User.Identity.IsAuthenticated)
            {
                dbUsers = await _userManager.Users.Where(x => x.UserName != User.Identity.Name).ToListAsync();
            }

            var users = new List<UserViewModel>();

            foreach (var dbUser in dbUsers)
            {
                var user = new UserViewModel
                {
                    Id = dbUser.Id,
                    Fullname = dbUser.FullName,
                    UserName = dbUser.UserName,
                    Email = dbUser.Email,
                    Role = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault(),
                    IsActive = dbUser.IsActive
                };

                users.Add(user);
            }

            return View(users);
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
 

            var changeRoleViewModel = new ChangeRoleViewModel
            {
                UserName = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                Roles = GetRoles()
            };

            return View(changeRoleViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, ChangeRoleViewModel changeRoleViewModel, string role)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
 
            var dbChangeRoleViewModel = new ChangeRoleViewModel
            {
                UserName = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                Roles = GetRoles()
            };
             

            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            string newRole = changeRoleViewModel.Role;
            if (oldRole != newRole)
            {
                var addResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Some problem exist");
                    return View(dbChangeRoleViewModel);
                }

                var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Some problem exist");
                    return View(dbChangeRoleViewModel);
                }
            }

            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Activity(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
 
            if (user == null)
                return NotFound();

            if (user.IsActive)
            {
                user.IsActive = false;
                
            }
            else
            {
                user.IsActive = true;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }


        public List<string> GetRoles()
        {
            List<string> roles = new List<string>();

            roles.Add(RoleConstants.AdminRole);
            roles.Add(RoleConstants.SupervisorRole);
            roles.Add(RoleConstants.MemberRole);

            return roles;
        }
    }
}
