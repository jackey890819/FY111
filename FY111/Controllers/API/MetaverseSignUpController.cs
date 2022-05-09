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
    public class MetaverseSignupController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public MetaverseSignupController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // POST: api/MetaverseSignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<MetaverseSignup>> PostMetaverseSignup(MetaverseSignup metaverseSignup)
        {

            var user = await _userManager.GetUserAsync(User);
            metaverseSignup.MemberId = user.Id;
            try
            {
                _context.MetaverseSignups.Add(metaverseSignup);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Sign up successfully." });
            }
            catch (DbUpdateException)   // 報名失敗：已經報名過
            {
                return BadRequest(new { uccess = false, message = "You have signed up the metaverse." });
            }
        }

        // DELETE: api/MetaverseSignUp/5
        [HttpDelete("{metaverseId}")]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<MetaverseSignup>> DeleteMetaverseSignup(int metaverseId)
        {
            var user = _userManager.GetUserAsync(User);
            var metaverseSignup = await _context.MetaverseSignups.FindAsync(user.Id, metaverseId);
            if (metaverseSignup == null)
            {
                return BadRequest(new { success = false, message = "You haven't signed in this metaverse." });
            }
            _context.MetaverseSignups.Remove(metaverseSignup);
            await _context.SaveChangesAsync();
            //return metaverseSignup;
            return Ok(new { success = true, message = "Delete signed in metaverse success." });
        }

        private bool MetaverseSignupExists(string id)
        {
            return _context.MetaverseSignups.Any(e => e.MemberId == id);
        }
    }
}
