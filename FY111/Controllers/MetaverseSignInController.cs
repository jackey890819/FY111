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
    public class MetaverseSignInController : ControllerBase
    {
        private readonly FY111Context _context;

        public MetaverseSignInController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/MetaverseSignIn
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MetaverseSignIn>>> GetMetaverseSignIns()
        //{
        //    return await _context.MetaverseSignIns.ToListAsync();
        //}

        // GET: api/MetaverseSignIn/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<MetaverseSignIn>> GetMetaverseSignIn(string id)
        //{
        //    var metaverseSignIn = await _context.MetaverseSignIns.FindAsync(id);

        //    if (metaverseSignIn == null)
        //    {
        //        return NotFound();
        //    }

        //    return metaverseSignIn;
        //}

        // PUT: api/MetaverseSignIn/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMetaverseSignIn(string id, MetaverseSignIn metaverseSignIn)
        //{
        //    if (id != metaverseSignIn.MemberId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(metaverseSignIn).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MetaverseSignInExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/MetaverseSignIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MetaverseSignIn>> PostMetaverseSignIn(MetaverseSignIn metaverseSignIn)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseSignIn.MemberId = user_id;
            _context.MetaverseSignIns.Add(metaverseSignIn);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Create Successfully" });
        }

        // DELETE: api/MetaverseSignIn/5
        [HttpDelete("id")]
        public async Task<ActionResult<MetaverseSignIn>> DeleteMetaverseSignIn(int id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var metaverseSignIn = await _context.MetaverseSignIns.FindAsync(id, user_id);
            if (metaverseSignIn == null)
            {
                return NotFound();
            }

            _context.MetaverseSignIns.Remove(metaverseSignIn);
            await _context.SaveChangesAsync();

            return metaverseSignIn;
        }

        private bool MetaverseSignInExists(string id)
        {
            return _context.MetaverseSignIns.Any(e => e.MemberId == id);
        }
    }
}
