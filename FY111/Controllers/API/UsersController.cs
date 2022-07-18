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
using System.Collections.Generic;
using FY111.Models.FY111;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers.API
{
    [Route("api/")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
        private readonly ApplicationSettings _appSettings;
        
        public UsersController(
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager,
            IOptions<ApplicationSettings> appSettings,
            FY111Context context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
        }

        //[HttpPost]
        //[Route("Register")]
        ////POST：/api/User/Register
        //public async Task<Object> PostUser(RegisterModel model)
        //{
        //    FY111User user = new FY111User(){ UserName = model.UserName };
        //    try
        //    {
        //        var exist = await _userManager.FindByNameAsync(user.UserName);
        //        if (exist == null)
        //        {
        //            var result = await _userManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                await _userManager.AddToRoleAsync(user, "NormalUser"); 
        //                return Ok(new
        //                {
        //                    success = true,
        //                    message = $"Create successed. UserName: {user.UserName}"
        //                });
        //            }
        //            List<string> message = new List<string>();
        //            foreach (var error in result.Errors)
        //            {
        //                message.Add(error.Description);
        //            }
        //            return Ok(new
        //            {
        //                success = false,
        //                message = message
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new 
        //            {
        //                success = false,
        //                message = "UserName exist."
        //            });
        //        }
        //    } catch (Exception ex)
        //    {
        //        return BadRequest(new {
        //            success = false,
        //            message = ex.Message
        //        });
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("monitorPlatformLogin")]
        //POST：/api/User/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (_signInManager.IsSignedIn(User))
                return BadRequest(new
                {
                    errors = "Login failed"
                });
                //return BadRequest(new { 
                //    success = false,
                //    message = "You are already logged in."
                //});
            var user = await _userManager.FindByNameAsync(model.account);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.password) && !_signInManager.IsSignedIn(User))
            {
                #region JWT
                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(new Claim[]
                //    {
                //        new Claim("UserID", user.Id.ToString())
                //    }),
                //    Expires = DateTime.UtcNow.AddDays(1),
                //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                //};
                //var tokenHandler = new JwtSecurityTokenHandler();
                //var secutityToken = tokenHandler.CreateToken(tokenDescriptor);
                //var token = tokenHandler.WriteToken(secutityToken);
                #endregion JWT

                var result = await _signInManager.PasswordSignInAsync(model.account, model.password, false, lockoutOnFailure: false);  // 登入
                await GenerateLoginLogAsync(user, (model.DeviceType!=0) ? model.DeviceType : 1);      // 存入Log data到資料庫
                //return await GetClass(user);        // 根據身分取得元宇宙列表
                return Ok(new { token = "todo..." });
                //return Ok(result);
            }
            else
            {
                return BadRequest(new
                {
                    errors = "Login failed"
                });
                //return BadRequest( new { 
                //    success = false,
                //    message = "Username or password is incorrect." 
                //});
            }
        }

        private async Task GenerateLoginLogAsync(FY111User user, int deviceType = 1)
        {
            LoginLog log = new LoginLog();
            log.MemberId = user.Id;
            log.DeviceType = deviceType;
            log.StartTime = DateTime.Now;
            _context.LoginLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        //private async Task<IActionResult> GetClass(FY111User user)
        //{
        //    string user_roles = (await _userManager.GetRolesAsync(user))[0];
        //    switch (user_roles)
        //    {
        //        case "NormalUser":
        //        case "GroupUser":
        //            var selected_list = await _context.ClassSignups
        //                        .Where(x => x.MemberId == user.Id)
        //                        .Select(x => x.ClassId).ToListAsync();
        //            var selected_class = await _context.Classes
        //                                    .Where(x => selected_list.Contains(x.Id)).ToListAsync();
        //            var @class = await _context.Classes
        //                            .Where(b => b.SignupEnabled == 1 && !selected_class.Contains(b)) // 列出尚未選擇並且可選擇的元宇宙
        //                            .ToListAsync();
        //            return Ok(new
        //            {
        //                selected_list = selected_class,
        //                none_selected_list = @class
        //            });
        //        case "MetaverseAdmin":
        //        case "SuperAdmin":
        //            var all_class = await _context.Classes.ToListAsync();
        //            return Ok(new
        //            {
        //                none_selected_list = all_class
        //            });
        //    }
        //    return BadRequest(new {
        //        success=false,
        //        message="Get metverse error."
        //    });
        //}

        [HttpPost]
        [Route("monitorPlatformLogout")]
        //POST：/api/User/Logout
        public async Task<IActionResult> Logout()
        {
            // 使用者未登入
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new{
                    errors = "Logout failed"
                });
            //return BadRequest(new { 
            //    success = false,
            //    message = "You haven't login system." 
            //});
            // 使用者已登入，執行登出
            await AddLogoutLog();
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successed." });
            //return Ok(new { 
            //    success = true,
            //    message = "Logout successed." 
            //});

        }
        private async Task AddLogoutLog()
        {
            var user_id = _userManager.GetUserId(User);
            LoginLog temp = _context.LoginLogs.FirstOrDefault(x => x.MemberId == user_id && x.EndTime == null);
            _context.Entry(temp).State = EntityState.Modified;
            temp.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
