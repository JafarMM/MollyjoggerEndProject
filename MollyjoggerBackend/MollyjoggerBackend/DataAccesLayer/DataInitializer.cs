using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MollyjoggerBackend.Data;

namespace MollyjoggerBackend.DataAccesLayer
{
    public class DataInitializer
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(AppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            await _dbContext.Database.MigrateAsync();

            #region Role Seed

            var roles = new List<string>
            {
                RoleConstants.AdminRole,
                RoleConstants.SupervisorRole,
                RoleConstants.MemberRole
            };

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            #endregion

            #region User Seed

            //Default Admin
            var user = new User
            {
                Email = "admin@gmail.com",
                UserName = "Admin",
                FullName = "Admin Admin"
            };

            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                await _userManager.CreateAsync(user, "Admin@123");
                await _userManager.AddToRoleAsync(user, RoleConstants.AdminRole);
            }

            #endregion
        }
    }
}
