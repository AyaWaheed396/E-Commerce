using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Services.UserService.Dto
{
    public static class UserManagerExtenion
    {
        public static async Task<AppUser?> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);

            return user;
        }
    }
}
