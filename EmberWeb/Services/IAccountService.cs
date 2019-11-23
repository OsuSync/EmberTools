using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Services
{
    public interface IAccountService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);
        Task<T> FindUserByEmail(string email);
        Task SignIn(T user);
        Task SignInAsync(T user, AuthenticationProperties properties, string authenticationMethod = null);
    }
}
