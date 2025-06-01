using Finance.Data;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance.Web.Initializers
{
    public static class IdentityInitializer
    {
        private static readonly string[] roles = { RolesEnum.Admin, RolesEnum.Customer };

        public static async Task InitAsync(IServiceProvider serviceProvider)
        {
            //run all migrations
            var db = serviceProvider.GetRequiredService<FinanceDbContext>();
            db.Database.Migrate();

            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            var roleExist = await roleManager.RoleExistsAsync(RolesEnum.Admin);
            //check if roles already initialized
            if (!roleExist)
            {
                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = serviceProvider
                        .GetRequiredService<UserManager<User>>();
            var configuration = serviceProvider
                        .GetRequiredService<IConfiguration>();
            var userDetailsService = serviceProvider.GetService<IUserDetailsService>();
            var users = await userManager.GetUsersInRoleAsync(RolesEnum.Admin);
            //check if admin user already initialized
            if (!users.Any())
            {
                var user = new User
                {
                    Email = configuration["Admin:Email"],
                    FirstName = configuration["Admin:FirstName"],
                    LastName = configuration["Admin:LastName"],
                    UserName = configuration["Admin:UserName"],
                };

                await userManager.CreateAsync(user, configuration["Admin:Password"]);
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, code);

                var existingUser = await userManager.FindByEmailAsync(configuration["Admin:Email"]);

                if (existingUser != null)
                {
                    var newUserDetails = new UserDetail
                    {
                        Email = configuration["Admin:Email"],
                        FirstName = configuration["Admin:FirstName"],
                        LastName = configuration["Admin:LastName"],
                        DisplayName = configuration["Admin:UserName"],
                        UserId = existingUser.Id,
                        Description = String.Empty,
                        Status = "Admin",
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await userDetailsService.CreateUserDetailAsync(newUserDetails);
                }

                await userManager.AddToRolesAsync(user, roles );
            }
        }
    }
}
