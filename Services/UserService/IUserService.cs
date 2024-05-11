using Core.IdentityEntities;
using Services.OrderService.Services.Dto;
using Services.UserService.Dto;
using System.Security.Claims;

namespace Services.UserService
{
    public interface IUserService
    {

        public Task<UserDto> Register(RegisterDto registerDto);
        public Task<UserDto> Login(LoginDto loginDto);

        public Task<UserDto> GetCurrentUser(string email);

        public Task<AddressDto> GetCurrentUserAddress(ClaimsPrincipal User);

        public Task<AppUser> UpdateUserAddress(ClaimsPrincipal User, AddressDto addressDto);





    }
}
