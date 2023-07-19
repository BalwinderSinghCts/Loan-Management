using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.DataBaseContext;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseDbContext _databaseDbContext = null;
        private readonly IConfiguration _configuration;
        private readonly AuthService _auth;
        private readonly IUserService _iUserService;
        public AccountController(DatabaseDbContext databaseDbContext ,IUserService userService, IConfiguration configuration)
        {
            _iUserService = userService;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _databaseDbContext = databaseDbContext;
        }
        [HttpPost("v{version:apiVersion}/userregister")]
        public IActionResult Register([FromBody] User user)
        {
            var userExists = _iUserService.UserExists(user.Email);
            if (userExists)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            user.Password = SecurePasswordHasherHelper.Hash(user.Password);
            _databaseDbContext.Users.Add(user);
            _databaseDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, "user created successfully");
        }
        [HttpPost("v{version:apiVersion}/userlogin")]
        public IActionResult Login(UserLoginVM user)
        {
            var userExists = _iUserService.GetUser(user.Email);
            if (userExists is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found with this Id");
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password!, userExists.Password))
            {
                return Unauthorized();
            }
            var claims = new[]
 {
   new Claim(JwtRegisteredClaimNames.Email, userExists.Email),
   new Claim(ClaimTypes.Email, userExists.Email),
   new Claim(ClaimTypes.Role, userExists.Role),
 };

            var token = _auth.GenerateAccessToken(claims);

            var tokenamin = new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                userid = userExists.Id,
                name = userExists.Name
            });

            return StatusCode(StatusCodes.Status200OK, tokenamin);
        }
    }
}
