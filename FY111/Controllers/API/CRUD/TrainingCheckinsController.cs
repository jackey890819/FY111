using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;

namespace FY111.Controllers.API.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingCheckinsController : ControllerBase
    {
        private readonly FY111Context _context;

        public TrainingCheckinsController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/TrainingCheckins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingCheckin>>> GetTrainingCheckins(
            [FromQuery] string memberId = "", [FromQuery] int trainingId = -1)
        {
            List<TrainingCheckin> result;
            if (memberId != "" && trainingId != -1)
            {
                result = await _context.TrainingCheckins
                    .Where(e => e.MemberId == memberId && e.TrainingId == trainingId)
                    .ToListAsync();
            }
            else if (memberId != "")
            {
                result = await _context.TrainingCheckins
                    .Where(e => e.MemberId == memberId)
                    .ToListAsync();
            }
            else if (trainingId != 1)
            {
                result = await _context.TrainingCheckins
                    .Where(e => e.TrainingId == trainingId)
                    .ToListAsync();
            }
            else
            {
                result = await _context.TrainingCheckins
                    .Take(10)
                    .ToListAsync();
            }
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return await _context.TrainingCheckins.ToListAsync();
        }



        // POST: api/TrainingCheckins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TrainingCheckin>> PostTrainingCheckin(TrainingCheckin trainingCheckin)
        {
            // 檢查重複報到
            var exist = await _context.TrainingCheckins
                .Where(e => e.MemberId == trainingCheckin.MemberId && e.TrainingId == trainingCheckin.TrainingId)
                .AnyAsync();
            if (exist)
                return BadRequest("已報到。");

            // 賦予報到時間
            trainingCheckin.Time = DateTime.Now;
            // 新增資料
            _context.TrainingCheckins.Add(trainingCheckin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TrainingCheckinExists(trainingCheckin.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTrainingCheckin", new { id = trainingCheckin.MemberId }, trainingCheckin);
        }

        // DELETE: api/TrainingCheckins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrainingCheckin>> DeleteTrainingCheckin(string id)
        {
            var trainingCheckin = await _context.TrainingCheckins.FindAsync(id);
            if (trainingCheckin == null)
            {
                return NotFound();
            }

            _context.TrainingCheckins.Remove(trainingCheckin);
            await _context.SaveChangesAsync();

            return trainingCheckin;
        }

        private bool TrainingCheckinExists(string id)
        {
            return _context.TrainingCheckins.Any(e => e.MemberId == id);
        }
    }
}
