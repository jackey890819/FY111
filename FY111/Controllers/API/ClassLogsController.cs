using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FY111.Dtos;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassLogsController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public ClassLogsController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/ClassLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassLogDto>>> GetClassLogs([FromQuery]string memberId="", [FromQuery]int classId=-1)
        {
            List<ClassLogDto> result = null;
            
            if (memberId != "" && classId != -1)
            {
                result = classLogToDto(await _context.ClassLogs
                    .Where(e => e.MemberId == memberId && e.ClassId == classId)
                    .ToListAsync());
            }

            if (result.Count()==0)
            {
                return NotFound();
            }

            return result;
        }


        // PUT: api/ClassLogs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutClassLog(int? classId=null, int? score=null)
        {
            if (classId == null)
                return BadRequest("請傳入classId");
            
            string memberId = _userManager.GetUserId(User);
            ClassLog classLog;
            try
            {
                classLog = await _context.ClassLogs
                    .Where(e => e.MemberId == memberId && e.ClassId == classId && e.EndTime == null)
                    .OrderByDescending(e => e.StartTime)
                    .SingleAsync();
            } catch
            {
                classLog = null;
                return BadRequest("登記結束時間失敗。");
            }
            classLog.EndTime = DateTime.Now;    // 紀錄離開時間
            if (score != null)                  // 檢查有無傳入分數資訊
                classLog.Score = score;         // 有則加入

            // 修改資料庫內容
            try
            {
                _context.ChangeTracker.Clear();
                _context.Entry(classLog).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/ClassLogs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassLog>> PostClassLog(int classId)
        {
            // 檢查課程
            bool checkClass = await _context.Classes
                .Where(e => e.Id == classId)
                .AnyAsync();
            if (!checkClass)
                return BadRequest("課程Id不存在");
            // 建立Log資料
            ClassLog classLog = new ClassLog
            {
                MemberId = _userManager.GetUserId(User),
                ClassId = classId,
                StartTime = DateTime.Now,
                EndTime = null,
                Score = null
            };
            // 於資料庫新增
            try
            {
                _context.ClassLogs.Add(classLog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClassLogExists(classLog.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetClassLog", new { id = classLog.MemberId }, classLog);
        }

        // DELETE: api/ClassLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClassLog>> DeleteClassLog(string id)
        {
            var classLog = await _context.ClassLogs.FindAsync(id);
            if (classLog == null)
            {
                return NotFound();
            }

            _context.ClassLogs.Remove(classLog);
            await _context.SaveChangesAsync();

            return classLog;
        }

        private bool ClassLogExists(string id)
        {
            return _context.ClassLogs.Any(e => e.MemberId == id);
        }

        private List<ClassLogDto> classLogToDto(List<ClassLog> list)
        {
            List<ClassLogDto> result = new List<ClassLogDto>();
            foreach (ClassLog classLog in list)
            {
                ClassLogDto dto = new ClassLogDto()
                {
                    MemberId = classLog.MemberId,
                    ClassId = classLog.ClassId,
                    StartTime = classLog.StartTime,
                    EndTime = classLog.EndTime,
                    Score = classLog.Score
                };
                result.Add(dto);
            }
            return result;
        }
    }
}
