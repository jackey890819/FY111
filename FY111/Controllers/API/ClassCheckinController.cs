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
    public class ClassCheckinController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public ClassCheckinController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // POST: api/ClassCheckin
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassCheckin>> PostClassCheckin(ClassCheckin classCheckin)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();

            var user = await _userManager.GetUserAsync(User);
            var hasSignup = await _context.ClassSignups
                .Where(x => x.ClassId == classCheckin.ClassId && x.MemberId == user.Id)
                .ToListAsync();
            if (!hasSignup.Any())
                return BadRequest(new
                {
                    success = false,
                    message = "You have not signed up this class."
                });
            try
            {
                classCheckin.MemberId = user.Id;
                classCheckin.Time = DateTime.Now;
                _context.ClassCheckins.Add(classCheckin);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "You have checked in." });
            }
            return Ok(new { success = true, message = "Create Successfully" });
        }

        [HttpDelete("{classId}")]
        public async Task<ActionResult<ClassCheckin>> DeleteClassCheckin(int classId)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();

            var user = await _userManager.GetUserAsync(User);
            
            var classCheckin = await _context.ClassCheckins.Where(x => x.MemberId == user.Id && x.ClassId == classId).ToListAsync();
            if (!classCheckin.Any())
            {
                return BadRequest(new { success = false, message = "You haven't checked in this class." });
                //return NotFound();
            }

            _context.ClassCheckins.Remove(classCheckin[0]);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Delete check in class success." });
        }

        private bool ClassCheckinExists(string id)
        {
            return _context.ClassCheckins.Any(e => e.MemberId == id);
        }
    }
}
