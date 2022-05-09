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
    public class MetaverseCheckinController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public MetaverseCheckinController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // POST: api/MetaverseCheckin
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MetaverseCheckin>> PostMetaverseCheckin(MetaverseCheckin metaverseCheckin)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();

            var user = await _userManager.GetUserAsync(User);
            var hasSignup = await _context.MetaverseSignups
                .Where(x => x.MetaverseId == metaverseCheckin.MetaverseId && x.MemberId == user.Id)
                .ToListAsync();
            if (!hasSignup.Any())
                return BadRequest(new
                {
                    success = false,
                    message = "You have not signed up this metaverse."
                });
            try
            {
                metaverseCheckin.MemberId = user.Id;
                metaverseCheckin.Time = DateTime.Now;
                _context.MetaverseCheckins.Add(metaverseCheckin);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "You have checked in." });
            }
            return Ok(new { success = true, message = "Create Successfully" });
        }

        [HttpDelete("{metaverseId}")]
        public async Task<ActionResult<MetaverseCheckin>> DeleteMetaverseCheckin(int metaverseId)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();

            var user = await _userManager.GetUserAsync(User);
            
            var metaverseCheckin = await _context.MetaverseCheckins.Where(x => x.MemberId == user.Id && x.MetaverseId == metaverseId).ToListAsync();
            if (!metaverseCheckin.Any())
            {
                return BadRequest(new { success = false, message = "You haven't checked in this metaverse." });
                //return NotFound();
            }

            _context.MetaverseCheckins.Remove(metaverseCheckin[0]);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Delete check in metaverse success." });
        }

        private bool MetaverseCheckinExists(string id)
        {
            return _context.MetaverseCheckins.Any(e => e.MemberId == id);
        }
    }
}
