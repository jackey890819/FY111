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
    public class CourseMembersController : ControllerBase
    {
        private readonly drive_courseContext _context;

        public CourseMembersController(drive_courseContext context)
        {
            _context = context;
        }

        // GET: api/CourseMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseMember>>> GetCourseMembers()
        {
            return await _context.CourseMembers.ToListAsync();
        }

        // GET: api/CourseMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseMember>> GetCourseMember(int id)
        {
            var courseMember = await _context.CourseMembers.FindAsync(id);

            if (courseMember == null)
            {
                return NotFound();
            }

            return courseMember;
        }

        // PUT: api/CourseMembers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseMember(int id, CourseMember courseMember)
        {
            if (id != courseMember.Id)
            {
                return BadRequest();
            }

            _context.Entry(courseMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseMemberExists(id))
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

        // POST: api/CourseMembers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CourseMember>> PostCourseMember(CourseMember courseMember)
        {
            _context.CourseMembers.Add(courseMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseMember", new { id = courseMember.Id }, courseMember);
        }

        // DELETE: api/CourseMembers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseMember>> DeleteCourseMember(int id)
        {
            var courseMember = await _context.CourseMembers.FindAsync(id);
            if (courseMember == null)
            {
                return NotFound();
            }

            _context.CourseMembers.Remove(courseMember);
            await _context.SaveChangesAsync();

            return courseMember;
        }

        private bool CourseMemberExists(int id)
        {
            return _context.CourseMembers.Any(e => e.Id == id);
        }
    }
}
