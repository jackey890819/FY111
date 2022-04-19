using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Net.Http;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly FY111Context _context;

        public MemberController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/Members
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        //{
        //    return await _context.Members.ToListAsync();
        //}

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        [HttpPost("app_logout/{id}")]
        public async Task<ActionResult<Member>> App_Logout(int id, Member member)
        {
            int MHDid = _context.MemberHasDevices.FirstOrDefault(x => x.MemberId == member.Id && x.DeviceId == id).Id;
            Log temp = _context.Logs.FirstOrDefault(x => x.MemberHasDeviceId == MHDid && x.EndTime == null);
            if (temp == null) return BadRequest();
            else
            {
                temp.EndTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return Content("Successful!!");
            }
        }

        [HttpPost("app_login/{id}")]
        public async Task<ActionResult<Member>> App_Login(int id, App_Login_Model model)
        {
            Member m = _context.Members.FirstOrDefault(m => m.Account == model.member.Account && m.Password == model.member.Password);
            if (m != null)
            {
                MemberHasDevice MHD = _context.MemberHasDevices.FirstOrDefault(x => x.MemberId == m.Id && x.DeviceId == id);
                int MHDid;
                if (MHD == null)
                {
                    MHD = new MemberHasDevice();
                    MHD.MemberId = m.Id;
                    MHD.DeviceId = id;
                    MHD.MacAddress = model.mac_address;
                    _context.MemberHasDevices.Add(MHD);
                    await _context.SaveChangesAsync();
                    MHDid = _context.MemberHasDevices.FirstOrDefault(x => x.MemberId == m.Id && x.DeviceId == id).Id;
                }
                else MHDid = MHD.Id;
                Log log = new Log();
                log.MemberHasDeviceId = MHDid;
                log.StartTime = DateTime.Now;
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
                return m;
            }
            else
            {
                return Content("帳號密碼錯誤");
            }
        }

        [HttpPost("web_login")]
        public async Task<ActionResult<Member>> Web_Login(Member member)
        {
            Member m = _context.Members.FirstOrDefault(m => m.Account == member.Account && m.Password == member.Password);
            if (m != null)
            {
                return m;
            }
            else
            {
                return Content("帳號密碼錯誤");
            }
        }

        [HttpPost("web_logout")]
        public async Task<ActionResult<Member>> Web_Logout()
        {
            return Content("Nothing happened.");
        }

        // POST: api/Members
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("register")]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            if (member == null) return BadRequest("Enter required fields");
            else if (_context.Members.Any(m => m.Account == member.Account)) return Content("Account is exists!!");
            else if (_context.Members.Any(m => m.Name == member.Name)) return Content("Name is exists!!");
            member.Permission = 2;
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return Content("Successful!!");
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return member;
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
