using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JwtAuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("LoginCheck")]
        [Authorize]
        public async Task<IActionResult> LoginCheck()
        {

            return Ok();
        }

        [HttpGet("IsSuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> IsSuperAdmin()
        {

            return Ok();
        }

        [HttpGet("IsClassAdmin")]
        [Authorize(Roles = "ClassAdmin")]
        public async Task<IActionResult> IsClassAdmin()
        {

            return Ok();
        }

        [HttpGet("IsGroupUser")]
        [Authorize(Roles = "GroupUser")]
        public async Task<IActionResult> IsGroupUser()
        {

            return Ok();
        }

        [HttpGet("IsNormalUser")]
        [Authorize(Roles = "NormalUser")]
        public async Task<IActionResult> IsNormalUser()
        {

            return Ok();
        }
    }
}
