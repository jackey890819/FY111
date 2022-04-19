using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.DriveCourse;

namespace FY111.Controllers.DriveCourse
{
    [Route("api/DriveCourse/[controller]")]
    [ApiController]
    public class ExaminationsController : ControllerBase
    {
        private readonly drive_courseContext _context;

        public ExaminationsController(drive_courseContext context)
        {
            _context = context;
        }

        // GET: api/Examinations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Examination>>> GetExaminations()
        {
            return await _context.Examinations.ToListAsync();
        }

        // GET: api/Examinations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Examination>> GetExamination(int id)
        {
            var examination = await _context.Examinations.FindAsync(id);

            if (examination == null)
            {
                return NotFound();
            }

            return examination;
        }

        // PUT: api/Examinations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamination(int id, Examination examination)
        {
            if (id != examination.Id)
            {
                return BadRequest();
            }

            _context.Entry(examination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExaminationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Examinations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Examination>> PostExamination(Examination examination)
        {
            _context.Examinations.Add(examination);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamination", new { id = examination.Id }, examination);
        }

        // DELETE: api/Examinations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Examination>> DeleteExamination(int id)
        {
            var examination = await _context.Examinations.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }

            _context.Examinations.Remove(examination);
            await _context.SaveChangesAsync();

            return examination;
        }

        private bool ExaminationExists(int id)
        {
            return _context.Examinations.Any(e => e.Id == id);
        }
    }
}
