using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Karem",
                    Email = "Karem@gmail.com",
                    UserName = "karemSayed",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Gamal",
                        Street = "89",
                        state = "Cairo",
                        City = "Maadi",
                        ZipCode = "92562"

                    }

                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
