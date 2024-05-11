using Core.IdentityEntities;
using E_Commerce.HandelResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Hepler;
using Services.OrderService.Services.Dto;
using Services.UserService;
using Services.UserService.Dto;
using System.Security.Claims;

namespace E_Commerce.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IUserService _UserService;
        private readonly UserManager<AppUser> _UserManager;
        

        public AccountController(IUserService userService, 
                                 UserManager<AppUser> userManager)
        {
            _UserService = userService;
            _UserManager = userManager;
           
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _UserService.Register(registerDto);
            if (user == null)
                return BadRequest(new ApiException(400, "Email Already Exist"));

            return Ok(user);

        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _UserService.Login(loginDto);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            return Ok(user);

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(u => u.Type == ClaimTypes.Email).Value;
            var email = User?.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized(new ApiResponse(401));

            var currentUser = await _UserService.GetCurrentUser(email);

            return Ok(currentUser);


        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var address = await _UserService.GetCurrentUserAddress(User);

            if (address == null)
                return NotFound(new ApiResponse(404, "Address not found"));

            return Ok(address);




        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            if (addressDto == null)
                return NotFound(new ApiResponse(404, "Please Enter The New Address"));

            var user = await _UserService.UpdateUserAddress(User, addressDto);

            var result = await _UserManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));


            return Ok(user.Address);




        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto email)
        {
            var user = await _UserManager.FindByEmailAsync(email.Email);
            if (user == null)
                return NotFound(new ApiResponse(404, "Wrong Email Address"));

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            var emailAddress = new Email()
            {
                To = email.Email,
                Subject = "Reset Password",
                Body = $"this is the link to Reset Your Password {link}"
            };

            var result = Email.SendEmail(emailAddress);
            if (!result)
                return BadRequest(new ApiResponse(500, "Email Not Sent"));

            

            return Ok(new ApiResponse(200, "Email Sent successfully"));

        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDto() { Token = token, Email = email };
            return Ok(model);

        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {

            var user = await _UserManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return NotFound(new ApiResponse(404, "Email not Found"));

            var result = await _UserManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok(new ApiResponse(200, "Password has Changed Successfully"));

        }
    }
}
