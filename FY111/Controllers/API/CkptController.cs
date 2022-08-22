using FY111.Areas.Identity.Data;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/")]
    [ApiController]
    public class CkptController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public CkptController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet("checkpoint")]
        public async Task<IActionResult> GetDisaster()
        {
            if (!_signInManager.IsSignedIn(User))
                return Unauthorized(new { errors = "Unauthorized" });
            var data = await _context.ClassLittleunits.Include(x => x.ClassUnitCkpts)
                .Select(x => new {
                    little_unit_code = x.Code,
                    checkpoints = x.ClassUnitCkpts.Select(x => new {
                        code = x.CkptId,
                        content = x.Content
                    }).ToList()
                }).ToListAsync();
            return Ok(new { data = data });
        }
    }
}
