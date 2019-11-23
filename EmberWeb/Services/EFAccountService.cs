using EmberWeb.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Services
{
    public class EFAccountService : IAccountService<EmberUser>
    {
        private readonly UserManager<EmberUser> _userManager;
        private readonly SignInManager<EmberUser> _signInManager;

        public EFAccountService(UserManager<EmberUser> userManager, SignInManager<EmberUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<EmberUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Task SignIn(EmberUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public Task SignInAsync(EmberUser user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, authenticationMethod);
        }

        public async Task<bool> ValidateCredentials(EmberUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
