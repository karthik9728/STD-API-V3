using MaxiShop.Application.Common;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationUser ApplicationUser;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                await _userManager.AddToRoleAsync(ApplicationUser, "ADMIN");
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
                return true;
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
