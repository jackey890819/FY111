using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using FY111.Dtos;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly FY111Context _context;

        public ClassesController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCRUDDto>>> GetClasses()
        {
            return await _context.Classes
                .Select(c => new ClassCRUDDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Ip = c.Ip,
                    Image = c.Image,
                    Content = c.Content,
                    SignupEnabled = c.SignupEnabled,
                    CheckinEnabled = c.CheckinEnabled,
                    Duration = c.Duration
                })
                .ToListAsync();
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassCRUDDto>> GetClass(int id)
        {
            ClassCRUDDto classDto =  await _context.Classes
                .Where(c => c.Id == id)
                .Select(c => new ClassCRUDDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Ip = c.Ip,
                    Image = c.Image,
                    Content = c.Content,
                    SignupEnabled = c.SignupEnabled,
                    CheckinEnabled = c.CheckinEnabled,
                    Duration = c.Duration

                }).SingleOrDefaultAsync();
            if (classDto == null)
                return NotFound();
            return Ok(classDto);
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<IActionResult> PutClass(int id, Class @class)
        {
            if (id != @class.Id)
            {
                return BadRequest("嘗試變更PK。");
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
                {
                    return NotFound("資料庫修改錯誤。");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Classes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = @class.Id }, @class);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<ActionResult<Class>> DeleteClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
