using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Identity;
using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers.API.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSignupsController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public TrainingSignupsController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/TrainingSignups/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingSignup>>> GetTrainingSignup
            ([FromQuery] string memberId = "", [FromQuery] int trainingId = -1)
        {
            List<TrainingSignup> result;
            if (memberId != "" && trainingId != -1)
            {
                result = await _context.TrainingSignups
                    .Where(e => e.MemberId == memberId && e.TrainingId == trainingId)
                    .ToListAsync();
            }
            else if (memberId != "")
            {
                result = await _context.TrainingSignups
                    .Where(e => e.MemberId == memberId)
                    .ToListAsync();
            }
            else if (trainingId != -1)
            {
                result = await _context.TrainingSignups
                    .Where(e => e.TrainingId == trainingId)
                    .ToListAsync();
            }
            else
            {
                result = await _context.TrainingSignups
                    .Take(10)
                    .ToListAsync();
            }

            if (result.Count() == 0)
            {
                return NotFound();
            }

            return result;
        }


        // POST: api/TrainingSignups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TrainingSignup>> PostTrainingSignup(TrainingSignup TrainingSignup)
        {
            if (!_signInManager.IsSignedIn(User))
                return Unauthorized();
            // 使用者檢查
            if (User.IsInRole("NormalUser"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.Id != TrainingSignup.MemberId)
                    return BadRequest("普通使用者無法新增他人的報名資料。");
            }
            // 重複報名檢查
            bool check = await _context.TrainingSignups
                .Where(e => e.Equals(TrainingSignup)).AnyAsync();
            if (check)
                return BadRequest("已進行報名");
            // 新增報名資料
            try
            {
                _context.TrainingSignups.Add(TrainingSignup);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TrainingSignupExists(TrainingSignup.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetTrainingSignup", new { id = TrainingSignup.MemberId }, TrainingSignup);
        }

        // DELETE: api/TrainingSignups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrainingSignup>> DeleteTrainingSignup(int id)
        {
            if (!_signInManager.IsSignedIn(User))
                return Unauthorized();
            var userId = _userManager.GetUserId(User);
            var TrainingSignup = await _context.TrainingSignups.Where(e => e.TrainingId == id && e.MemberId == userId).SingleOrDefaultAsync();
            if (TrainingSignup == null)
            {
                return NotFound();
            }

            _context.TrainingSignups.Remove(TrainingSignup);
            await _context.SaveChangesAsync();

            return TrainingSignup;
        }

        private bool TrainingSignupExists(string id)
        {
            return _context.TrainingSignups.Any(e => e.MemberId == id);
        }
    }
}
