using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using app.Interfaces;
using app.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AuthenticationProperties _authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
        };

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private async Task<List<Claim>> GetClaimsList(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;
        }
        public async Task<Tuple<ClaimsIdentity, AuthenticationProperties>> SignUpUserAsync(string username, string email, string password)
        {
            if (await _userManager.Users.AnyAsync(user => user.UserName == username))
                throw new Exception("Username is taken");

            if (await _userManager.Users.AnyAsync(user => user.Email == email))
                throw new Exception("Email is taken");

            var user = new User 
            { 
                Email = email,
                UserName = username 
            };

            var wasUserSaved = await _userManager.CreateAsync(user, password);
            if (!wasUserSaved.Succeeded)
                throw new Exception("something gone wrong try later");
            
            var wereRolesGranted = await _userManager.AddToRolesAsync(user, new[] { "Member" });
            if (!wereRolesGranted.Succeeded)
                throw new Exception("problem granting role");

            return Tuple.Create(
                new ClaimsIdentity(await GetClaimsList(user), CookieAuthenticationDefaults.AuthenticationScheme), 
                _authProperties);
        }

        public async Task<Tuple<ClaimsIdentity, AuthenticationProperties>> SignInUserAsync(string email, string password)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(
                user => user.Email == email);

            if (user == null)
                throw new Exception("Invalid Username");

            var wasUserSignedIn = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!wasUserSignedIn.Succeeded)
                throw new Exception("Invalid password");

            return Tuple.Create(
                new ClaimsIdentity(await GetClaimsList(user), CookieAuthenticationDefaults.AuthenticationScheme), 
                _authProperties);
        }
    }
}
