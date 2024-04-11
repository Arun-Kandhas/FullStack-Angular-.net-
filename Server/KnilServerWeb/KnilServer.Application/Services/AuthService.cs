using KnilServer.Application.Common;
using KnilServer.Application.InputModel;
using KnilServer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private ApplicationUser ApplicationUser;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;            
            ApplicationUser = new();
            _config = configuration;
        }
             
        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            ApplicationUser.FirstName = register.FirstName;
            ApplicationUser.LastName = register.LastName;
            ApplicationUser.Email = register.Email;
            ApplicationUser.UserName = register.Email;
            

            var result = await _userManager.CreateAsync(ApplicationUser,register.Password);

            if(result.Succeeded)
            {
                return null;
            }

            return result.Errors;
        }

        public async Task<object> Login(Login login)
        {
            ApplicationUser = await _userManager.FindByEmailAsync(login.Email)!;

            if (ApplicationUser == null)
            {
                return "Invalid User Credential";
            }
            var result = await _signInManager.PasswordSignInAsync(ApplicationUser, login.Password, isPersistent: true, lockoutOnFailure: true);
            var isValidCredential = await _userManager.CheckPasswordAsync(ApplicationUser, login.Password);

            if (result.Succeeded)
            {
                var token = await GenerateToken();
                LoginResponse loginResponse = new LoginResponse
                {
                    UserId = login.Email,
                    Token = token
                };

                return loginResponse;

            }
            else
            {
                if (result.IsLockedOut)
                {
                    return "Your Account has been Lockec, Please Contact System Admin!!";
                }
                if (result.IsNotAllowed)
                {
                    return "Please Verify Email Address!";
                }
                if (isValidCredential == false)
                {
                    return "Invalid Password!!!";
                }
                else
                {
                    return "Login Failed!";
                }
            }
        }
        public async Task<string> GenerateToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSetting:securitykey"]!));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(ApplicationUser);

            var rolesClaim = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,ApplicationUser.Email!)
            }.Union(rolesClaim).ToList();

            var token = new JwtSecurityToken
                (
                    issuer: _config["JWTSetting:Issuer"],
                   audience: _config["JWTSetting:Audience"],
                   claims: claims,
                   signingCredentials: signinCredentials,
                   expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWTSetting:DurationInMinutes"]))

                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
