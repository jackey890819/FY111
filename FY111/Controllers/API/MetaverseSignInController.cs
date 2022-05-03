using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FY111.Areas.Identity.Data;

namespace FY111.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseSignInController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public MetaverseSignInController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // POST: api/MetaverseSignIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MetaverseSignIn>> PostMetaverseSignIn(MetaverseSignIn metaverseSignIn)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseSignIn.MemberId = user_id;
            try
            {
                _context.MetaverseSignIns.Add(metaverseSignIn);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Sign in successfully." });
            }
            catch (DbUpdateException)   // 報名失敗：已經報名過
            {
                return BadRequest(new { message = "You have signed in the metaverse." });
            }
        }

        // DELETE: api/MetaverseSignIn/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MetaverseSignIn>> DeleteMetaverseSignIn(int id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var metaverseSignIn = await _context.MetaverseSignIns.FindAsync(user_id, id);
            if (metaverseSignIn == null)
            {
                return BadRequest(new { success = false, message = "You haven't signed in this metaverse." });
            }
            _context.MetaverseSignIns.Remove(metaverseSignIn);
            await _context.SaveChangesAsync();
            //return metaverseSignIn;
            return Ok(new { success = true, message = "Delete signed in metaverse success." });
        }

        private bool MetaverseSignInExists(string id)
        {
            return _context.MetaverseSignIns.Any(e => e.MemberId == id);
        }
    }
}
