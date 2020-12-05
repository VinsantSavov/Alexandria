namespace Alexandria.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = await userManager.FindByNameAsync(GlobalConstants.AdministratorUsername);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = GlobalConstants.AdministratorUsername,
                    Email = GlobalConstants.AdministratorEmail,
                    ProfilePicture = GlobalConstants.AdministratorProfilePicture,
                    Gender = GenderType.Male,
                };

                var result = await userManager.CreateAsync(admin, GlobalConstants.AdministratorPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                var roleResult = await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
