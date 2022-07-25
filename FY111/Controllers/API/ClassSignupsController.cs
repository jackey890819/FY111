//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using FY111.Models.FY111;
//using Microsoft.AspNetCore.Identity;
//using FY111.Areas.Identity.Data;
//using Microsoft.AspNetCore.Authorization;

//namespace FY111.Controllers.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ClassSignupsController : ControllerBase
//    {
//        private readonly FY111Context _context;
//        private UserManager<FY111User> _userManager;
//        private SignInManager<FY111User> _signInManager;

//        public ClassSignupsController(
//            FY111Context context, 
//            UserManager<FY111User> userManager,
//            SignInManager<FY111User> signInManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        // GET: api/ClassSignups/5
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ClassSignup>>> GetClassSignup
//            ([FromQuery]String memberId="", [FromQuery]int classId=-1)
//        {
//            List<ClassSignup> result;
//            if (memberId !="" && classId != -1)
//            {
//                result = await _context.ClassSignups
//                    .Where(e => e.MemberId == memberId && e.ClassId == classId)
//                    .ToListAsync();
//            } else if (memberId != "")
//            {
//                result = await _context.ClassSignups
//                    .Where(e => e.MemberId == memberId)
//                    .ToListAsync();
//            }else if (classId != -1)
//            {
//                result = await _context.ClassSignups
//                    .Where(e => e.ClassId == classId)
//                    .ToListAsync();
//            }
//            else
//            {
//                result = await _context.ClassSignups
//                    .Take(10)
//                    .ToListAsync();
//            }

//            if (result.Count() == 0)
//            {
//                return NotFound();
//            }

//            return result;
//        }


//        // POST: api/ClassSignups
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for
//        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        [HttpPost]
//        public async Task<ActionResult<ClassSignup>> PostClassSignup(ClassSignup classSignup)
//        {
//            if (!_signInManager.IsSignedIn(User))
//                return Unauthorized();
//            // 使用者檢查
//            if (User.IsInRole("NormalUser"))
//            {
//                var user = await _userManager.GetUserAsync(User);
//                if (user.Id != classSignup.MemberId)
//                    return BadRequest("普通使用者無法新增他人的報名資料。");
//            }
//            // 重複報名檢查
//            bool check = await _context.ClassSignups
//                .Where(e => e.Equals(classSignup)).AnyAsync();
//            if (check)
//                return BadRequest("已進行報名");
//            // 新增報名資料
//            try
//            {
//                _context.ClassSignups.Add(classSignup);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (ClassSignupExists(classSignup.MemberId))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return CreatedAtAction("GetClassSignup", new { id = classSignup.MemberId }, classSignup);
//        }

//        // DELETE: api/ClassSignups/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<ClassSignup>> DeleteClassSignup(int id)
//        {
//            if (!_signInManager.IsSignedIn(User))
//                return Unauthorized();
//            var userId = _userManager.GetUserId(User);
//            var classSignup = await _context.ClassSignups.Where(e => e.ClassId == id && e.MemberId == userId).SingleOrDefaultAsync();
//            if (classSignup == null)
//            {
//                return NotFound();
//            }

//            _context.ClassSignups.Remove(classSignup);
//            await _context.SaveChangesAsync();

//            return classSignup;
//        }

//        private bool ClassSignupExists(string id)
//        {
//            return _context.ClassSignups.Any(e => e.MemberId == id);
//        }
//    }
//}
