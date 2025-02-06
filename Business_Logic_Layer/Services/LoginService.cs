using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.ContextInterfaces;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business_Logic_Layer.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IUserContext _userContext;

        public LoginService(IUserRepository userRepository, SignInManager<ApplicationUser> signInManager, IConfiguration config, UserManager<ApplicationUser> userManager, IUserContext userContext)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _config = config;
            _userManager = userManager;
            _userContext = userContext;
        }

        public async Task<LoginResult> LoginUser(LoginModel login)
        {

            var user = await _userManager.FindByNameAsync(login.Email);

            ValidateUser(user);

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (result.Succeeded == true)
            {
                var loginResult = new LoginResult
                {
                    User = user,
                    AccessToken = GenerateAccessToken(user.Email),
                };

                _userContext.UserId = user.Id;

                var userWithRole = await _userRepository.GetUserwithRoleById(user.Id);

                loginResult.Role = userWithRole.UserRole.RoleId;

                return loginResult;
            }
            else
            {
                throw new Exception("parola gresita");
            }
        }

        private void ValidateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new Exception("nu am gasit");
            }
        }

        public string GenerateAccessToken(string mail)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(symmetricKey,
            SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(issuer: "SafeScriptDB",
              expires: DateTime.Now.AddHours(2),
              signingCredentials: signingCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return accessToken;
        }

        private string GenerateToken(List<Claim> claims, DateTime expirationDate, string issuer, string audience, string secret)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(symmetricKey,
            SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims,
              expires: expirationDate,
              signingCredentials: signingCredentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtToken;
        }
    }
}
