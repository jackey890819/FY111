using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using FY111.Areas.Identity.Data;
using FY111.Models.FY111User;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using FY111.Models;

namespace FY111.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
        private readonly ApplicationSettings _appSettings;
        
        public UserController(
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager,
            IOptions<ApplicationSettings> appSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        //POST：/api/User/Register
        public async Task<Object> PostUser(RegisterModel model)
        {
            FY111User user = new FY111User()
            {
                UserName = model.UserName
            };
            try
            {
                var exist = await _userManager.FindByNameAsync(user.UserName);
                if (exist == null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "NormalUser");
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("UserName exist.");
                }

            } catch (Exception ex)
            {
                return BadRequest(model);
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST：/api/User/Login
        public async Task<Object> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var secutityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(secutityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }
        }
    }
}
