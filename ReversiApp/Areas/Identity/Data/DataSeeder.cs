using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Areas.Identity.Data
{
    public static class DataSeeder
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Player").Result)
            {
                IdentityRole idRole = new IdentityRole();
                idRole.Name = "Player";
                idRole.NormalizedName = "PLAYER";
                IdentityResult idResult = roleManager.CreateAsync(idRole).Result;
            }
            if (!roleManager.RoleExistsAsync("Mod").Result)
            {
                IdentityRole idRole = new IdentityRole();
                idRole.Name = "Mod";
                idRole.NormalizedName = "MOD";
                IdentityResult idResult = roleManager.CreateAsync(idRole).Result;
            }
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole idRole = new IdentityRole();
                idRole.Name = "Admin";
                idRole.NormalizedName = "ADMIN";
                IdentityResult idResult = roleManager.CreateAsync(idRole).Result;
            }
        }

        private static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                User adminUser = new User()
                {
                    Token = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    Email = "admin@admin.com",
                    UserName = "admin",
                    Archived = false
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "Admintje1!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }

            if (userManager.FindByNameAsync("mod").Result == null)
            {
                User modUser = new User
                {
                    Token = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    Email = "mod@mod.com",
                    UserName = "mod",
                    Archived = false
                };

                IdentityResult result = userManager.CreateAsync(modUser, "Moderatortje1!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(modUser, "Mod").Wait();
                }
            }
        }
    }

    
}
