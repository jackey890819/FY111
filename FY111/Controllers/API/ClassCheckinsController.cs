using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassCheckinsController : ControllerBase
    {
        private readonly FY111Context _context;

        public ClassCheckinsController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/ClassCheckins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCheckin>>> GetClassCheckins(
            [FromQuery]String memberId="", [FromQuery] int classId=-1)
        {
            List<ClassCheckin> result;
            if (memberId != "" && classId != -1)
            {
                result = await _context.ClassCheckins
                    .Where(e => e.MemberId == memberId && e.TrainingId == classId)
                    .ToListAsync();
            } else if (memberId != "")
            {
                result = await _context.ClassCheckins
                    .Where(e => e.MemberId == memberId)
                    .ToListAsync();
            } else if (classId != 1)
            {
                result = await _context.ClassCheckins
                    .Where(e => e.TrainingId == classId)
                    .ToListAsync();
            } else
            {
                result = await _context.ClassCheckins
                    .Take(10)
                    .ToListAsync();
            }
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return await _context.ClassCheckins.ToListAsync();
        }



        // POST: api/ClassCheckins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassCheckin>> PostClassCheckin(ClassCheckin classCheckin)
        {
            // 檢查重複報到
            var exist = await _context.ClassCheckins
                .Where(e => e.MemberId == classCheckin.MemberId && e.TrainingId == classCheckin.TrainingId)
                .AnyAsync();
            if (exist)
                return BadRequest("已報到。");

            // 賦予報到時間
            classCheckin.Time = DateTime.Now;
            // 新增資料
            _context.ClassCheckins.Add(classCheckin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClassCheckinExists(classCheckin.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClassCheckin", new { id = classCheckin.MemberId }, classCheckin);
        }

        // DELETE: api/ClassCheckins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClassCheckin>> DeleteClassCheckin(string id)
        {
            var classCheckin = await _context.ClassCheckins.FindAsync(id);
            if (classCheckin == null)
            {
                return NotFound();
            }

            _context.ClassCheckins.Remove(classCheckin);
            await _context.SaveChangesAsync();

            return classCheckin;
        }

        private bool ClassCheckinExists(string id)
        {
            return _context.ClassCheckins.Any(e => e.MemberId == id);
        }
    }
}
