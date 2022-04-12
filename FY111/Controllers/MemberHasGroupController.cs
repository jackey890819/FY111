using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberHasGroupController : ControllerBase
    {
        private readonly FY111Context _context;

        public MemberHasGroupController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/MemberHasGroup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberHasGroup>>> GetMemberHasGroups()
        {
            return await _context.MemberHasGroups.ToListAsync();
        }

        // GET: api/MemberHasGroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberHasGroup>> GetMemberHasGroup(int id)
        {
            var memberHasGroup = await _context.MemberHasGroups.FindAsync(id);

            if (memberHasGroup == null)
            {
                return NotFound();
            }

            return memberHasGroup;
        }

        // PUT: api/MemberHasGroup/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberHasGroup(int id, MemberHasGroup memberHasGroup)
        {
            if (id != memberHasGroup.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(memberHasGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberHasGroupExists(id))
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

        // POST: api/MemberHasGroup
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MemberHasGroup>> PostMemberHasGroup(MemberHasGroup memberHasGroup)
        {
            _context.MemberHasGroups.Add(memberHasGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MemberHasGroupExists(memberHasGroup.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMemberHasGroup", new { id = memberHasGroup.MemberId }, memberHasGroup);
        }

        // DELETE: api/MemberHasGroup/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MemberHasGroup>> DeleteMemberHasGroup(int id)
        {
            var memberHasGroup = await _context.MemberHasGroups.FindAsync(id);
            if (memberHasGroup == null)
            {
                return NotFound();
            }

            _context.MemberHasGroups.Remove(memberHasGroup);
            await _context.SaveChangesAsync();

            return memberHasGroup;
        }

        private bool MemberHasGroupExists(int id)
        {
            return _context.MemberHasGroups.Any(e => e.MemberId == id);
        }
    }
}
