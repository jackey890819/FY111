using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseCheckinController : ControllerBase
    {
        private readonly FY111Context _context;

        public MetaverseCheckinController(FY111Context context)
        {
            _context = context;
        }


        // POST: api/MetaverseCheckin
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MetaverseCheckin>> PostMetaverseCheckin(MetaverseCheckin metaverseCheckin)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseCheckin.MemberId = user_id;
            metaverseCheckin.Time = DateTime.Now;
            _context.MetaverseCheckins.Add(metaverseCheckin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Create Successfully" });
        }

        // DELETE: api/metaverseCheckin/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MetaverseCheckin>> DeleteMetaverseCheckin(string id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var metaverseCheckin = await _context.MetaverseCheckins.FindAsync(id, user_id);
            if (metaverseCheckin == null)
            {
                return NotFound();
            }

            _context.MetaverseCheckins.Remove(metaverseCheckin);
            await _context.SaveChangesAsync();

            return metaverseCheckin;
        }

        private bool MetaverseCheckinExists(string id)
        {
            return _context.MetaverseCheckins.Any(e => e.MemberId == id);
        }
    }
}
