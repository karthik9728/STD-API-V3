using MaxiShop.Application.Common;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MaxiShop.Application.ViewModels;

namespace MaxiShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private ApplicationUser ApplicationUser;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            ApplicationUser = new();
        }

        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            ApplicationUser.FirstName = register.FristName;
            ApplicationUser.LastName = register.LastName;
            ApplicationUser.UserName = register.EmailAddress;
            ApplicationUser.Email = register.EmailAddress;

            var result = await _userManager.CreateAsync(ApplicationUser, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(ApplicationUser, "CUSTOMER");
            }

            return result.Errors;
        }


        public async Task<object> Login(Login login)
        {
            ApplicationUser = await _userManager.FindByEmailAsync(login.EmailAddress);

            if (ApplicationUser == null)
            {
                return "Invalid Email Address";
            }

            var result = await _signInManager.PasswordSignInAsync(ApplicationUser, login.Password, isPersistent: true, lockoutOnFailure: true);

            var isValidCredential = await _userManager.CheckPasswordAsync(ApplicationUser, login.Password);

            if (result.Succeeded)
            {
                var token = await GenerateToken();

                LoginResponse loginResponse = new LoginResponse()
                {
                    UserId = ApplicationUser.Id,
                    Token = token
                };

                return loginResponse;
            }
            else
            {
                if (result.IsLockedOut)
                {
                    return "You are Account is Locked,Contact System Admin";
                }
                if (result.IsNotAllowed)
                {
                    return "Please Verfiy Email Address";
                }
                if (isValidCredential == false)
                {
                    return "Invalid Password";
                }
                else
                {
                    return "Login Failed";
                }
            }
        }

        public async Task<string> GenerateToken()
        {
            //getting security key from appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            //digital signature for security key with SecurityAlgorithms
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //get roles of user
            var roles = await _userManager.GetRolesAsync(ApplicationUser);

            //Converting Roles into claims
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,ApplicationUser.Email)
            }.Union(roleClaims).ToList();

            var token = new JwtSecurityToken
                (
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims:claims,
                signingCredentials:signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]))
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> IsUserExists(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
