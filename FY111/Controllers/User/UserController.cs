using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using FY111.Areas.Identity.Data;
using FY111.Models.FY111User;
using System;

namespace FY111.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
        
        public UserController(
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        //POST：/api/User/Register
        public async Task<Object> PostUser(UserModel model)
        {
            FY111User user = new FY111User()
            {
                UserName = model.UserName,
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
                        //return Created("", model);
                        return Ok(result);
                    }
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
            return BadRequest(model);
        }
    }
}
