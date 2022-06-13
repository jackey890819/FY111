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
    public class ClassSignUpController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public ClassSignUpController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // POST: api/ClassSignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassSignup>> PostClassSignup(ClassSignup classSignup)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();
            var user = await _userManager.GetUserAsync(User);
            classSignup.MemberId = user.Id;
            try
            {
                _context.ClassSignups.Add(classSignup);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Sign up successfully." });
            }
            catch (DbUpdateException)   // 報名失敗：已經報名過
            {
                return BadRequest(new { success = false, message = "You have signed up this class." });
            }
        }

        // DELETE: api/ClassSignUp/5
        [HttpDelete("{classId}")]
        public async Task<ActionResult<ClassSignup>> DeleteClassSignup(int classId)
        {
            if (!_signInManager.IsSignedIn(User))
                //return Unauthorized();
                return BadRequest(new { success = false, message = "Please log in." });
            if (!User.IsInRole("NormalUser"))
                return Forbid();
            var user = await _userManager.GetUserAsync(User);
            //var classSignup = await _context.ClassSignups.FindAsync( user.Id, classId);
            var classSignup = await _context.ClassSignups.Where(x => x.ClassId == classId && x.MemberId == user.Id).ToListAsync();
            if (!classSignup.Any())
            {
                return BadRequest(new { success = false, message = "You haven't signed up this class." });
            }
            _context.ClassSignups.Remove(classSignup[0]);
            await _context.SaveChangesAsync();
            //return classSignup;
            return Ok(new { success = true, message = "Delete signed up class success." });
        }

        private bool ClassSignupExists(string id)
        {
            return _context.ClassSignups.Any(e => e.MemberId == id);
        }
    }
}
