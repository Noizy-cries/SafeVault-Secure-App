using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SafeVault.Models;

namespace SafeVault.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RegisterAsync(string username, string email, string password, string firstName, string lastName);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                user = await _userManager.FindByEmailAsync(username);

            if (user == null || !user.IsActive)
                return AuthResult.Failed("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (result.Succeeded)
            {
                user.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var token = _tokenService.GenerateToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                return AuthResult.Successful(token, user.Id, user.UserName, user.Email, roles);
            }

            if (result.IsLockedOut)
                return AuthResult.Failed("Account locked. Try again later.");

            return AuthResult.Failed("Invalid credentials");
        }

        public async Task<AuthResult> RegisterAsync(string username, string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return AuthResult.Failed(string.Join(", ", result.Errors));

            await _userManager.AddToRoleAsync(user, "User");

            var token = _tokenService.GenerateToken(user);
            return AuthResult.Successful(token, user.Id, user.UserName, user.Email, new[] { "User" });
        }
    }

    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public static AuthResult Successful(string token, string userId, string username, string email, IEnumerable<string> roles)
        {
            return new AuthResult
            {
                Succeeded = true,
                Message = "Authentication successful",
                Token = token,
                UserId = userId,
                Username = username,
                Email = email,
                Roles = roles
            };
        }

        public static AuthResult Failed(string message)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = message
            };
        }
    }
}