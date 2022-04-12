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
    public class MemberHasDeviceController : ControllerBase
    {
        private readonly FY111Context _context;

        public MemberHasDeviceController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/MemberHasDevice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberHasDevice>>> GetMemberHasDevices()
        {
            return await _context.MemberHasDevices.ToListAsync();
        }

        // GET: api/MemberHasDevice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberHasDevice>> GetMemberHasDevice(int id)
        {
            var memberHasDevice = await _context.MemberHasDevices.FindAsync(id);

            if (memberHasDevice == null)
            {
                return NotFound();
            }

            return memberHasDevice;
        }

        // PUT: api/MemberHasDevice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberHasDevice(int id, MemberHasDevice memberHasDevice)
        {
            if (id != memberHasDevice.DeviceType)
            {
                return BadRequest();
            }

            _context.Entry(memberHasDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberHasDeviceExists(id))
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

        // POST: api/MemberHasDevice
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MemberHasDevice>> PostMemberHasDevice(MemberHasDevice memberHasDevice)
        {
            _context.MemberHasDevices.Add(memberHasDevice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MemberHasDeviceExists(memberHasDevice.DeviceType))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMemberHasDevice", new { id = memberHasDevice.DeviceType }, memberHasDevice);
        }

        // DELETE: api/MemberHasDevice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MemberHasDevice>> DeleteMemberHasDevice(int id)
        {
            var memberHasDevice = await _context.MemberHasDevices.FindAsync(id);
            if (memberHasDevice == null)
            {
                return NotFound();
            }

            _context.MemberHasDevices.Remove(memberHasDevice);
            await _context.SaveChangesAsync();

            return memberHasDevice;
        }

        private bool MemberHasDeviceExists(int id)
        {
            return _context.MemberHasDevices.Any(e => e.DeviceType == id);
        }
    }
}
