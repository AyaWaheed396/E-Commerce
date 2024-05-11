using AutoMapper;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Services.OrderService.Services.Dto;
using Services.TokenService.Interface;
using Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly ITokenService _TokenService;
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly IMapper _Mapper;

        public UserService(UserManager<AppUser> userManager,
                           ITokenService tokenService,
                           SignInManager<AppUser> signInManager,
                           IMapper mapper)
        {
            _UserManager = userManager;
            _TokenService = tokenService;
            _SignInManager = signInManager;
            _Mapper = mapper;
        }

        

        public async Task<UserDto> GetCurrentUser(string email)
        {
            var user = await _UserManager.FindByEmailAsync(email);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email
            };
        }

        public async Task<AddressDto> GetCurrentUserAddress(ClaimsPrincipal User)
        {
            var user = await _UserManager.FindUserWithAddressByEmailAsync(User);

            if (user == null)
                return null;

            var addressDto = _Mapper.Map<AddressDto>(user.Address);

            return addressDto;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return null;
            var result = await _SignInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return null;

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _TokenService.CreateToken(user)
            };
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = await _UserManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
                return null;

            var appUser = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],

            };
            var result = await _UserManager.CreateAsync(appUser, registerDto.Password);

            if (!result.Succeeded)
                return null;

            return new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = _TokenService.CreateToken(appUser)
            };


        }

        public async Task<AppUser> UpdateUserAddress(ClaimsPrincipal User, AddressDto addressDto)
        {
            var user = await _UserManager.FindUserWithAddressByEmailAsync(User);
            var address = _Mapper.Map<Address>(addressDto);
            user.Address = address;

            return user;


        }
    }
}
