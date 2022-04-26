/*using System;
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
    public class DriveCourseUsersController : ControllerBase
    {
        private readonly drive_courseContext _context;

        public DriveCourseUsersController(drive_courseContext context)
        {
            _context = context;
        }

        // GET: api/DriveCourseUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriveCourseUser>>> GetDriveCourseUsers()
        {
            return await _context.DriveCourseUsers.ToListAsync();
        }

        // GET: api/DriveCourseUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriveCourseUser>> GetDriveCourseUser(int id)
        {
            var DriveCourseUser = await _context.DriveCourseUsers.FindAsync(id);

            if (DriveCourseUser == null)
            {
                return NotFound();
            }

            return DriveCourseUser;
        }

        // PUT: api/DriveCourseUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriveCourseUser(int id, DriveCourseUser DriveCourseUser)
        {
            if (id != DriveCourseUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(DriveCourseUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriveCourseUserExists(id))
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

        // POST: api/DriveCourseUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DriveCourseUser>> PostDriveCourseUser(DriveCourseUser DriveCourseUser)
        {
            _context.DriveCourseUsers.Add(DriveCourseUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriveCourseUser", new { id = DriveCourseUser.Id }, DriveCourseUser);
        }

        // DELETE: api/DriveCourseUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DriveCourseUser>> DeleteDriveCourseUser(int id)
        {
            var DriveCourseUser = await _context.DriveCourseUsers.FindAsync(id);
            if (DriveCourseUser == null)
            {
                return NotFound();
            }

            _context.DriveCourseUsers.Remove(DriveCourseUser);
            await _context.SaveChangesAsync();

            return DriveCourseUser;
        }

        private bool DriveCourseUserExists(int id)
        {
            return _context.DriveCourseUsers.Any(e => e.Id == id);
        }
    }
}
*/