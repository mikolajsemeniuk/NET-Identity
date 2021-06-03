using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace app.Interfaces
{
    public interface IUserRepository
    {
        Task<Tuple<ClaimsIdentity, AuthenticationProperties>> SignUpUserAsync(string username, string email, string password);
        Task<Tuple<ClaimsIdentity, AuthenticationProperties>> SignInUserAsync(string email, string password);
    }
}