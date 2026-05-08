//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.IdentityModel.JsonWebTokens;
//using Microsoft.IdentityModel.Tokens;

//namespace CollectorApp.Api.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IConfiguration _configuration;

//        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _configuration = configuration;
//        }
//        public async Task<AuthResponseDto> LoginAsync(LoginModel model)
//        {
//            var user = await _userManager.FindByEmailAsync(model.Email);

//            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
//            {
//                throw new BadRequestException("Invalid login attempt.");
//            }

//            var authClaims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, user.Email!),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//        };

//            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                expires: DateTime.Now.AddHours(3),
//                claims: authClaims,
//                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
//            );

//            return new AuthResponseDto
//            {
//                Token = new JwtSecurityTokenHandler().WriteToken(token),
//                Expiration = token.ValidTo
//            };
//        }

//        public async Task RegisterAsync(LoginModel model)
//        {
//            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
//                throw new BadRequestException(errors);
//            }
//        }
//    }

//}