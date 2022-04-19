using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Text.Json;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly FY111Context _context;

        public FriendController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/Friend
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriends()
        {
            return await _context.Friends.ToListAsync();
        }

        // GET: api/Friend/5
        [HttpGet("get_friend_list/{id}")]
        public async Task<string> GetFriend(int id)
        {
            var friends = await _context.Friends.Where(f => f.MemberId == id).Select(f => f.MemberId1).ToListAsync();
            friends.Concat(await _context.Friends.Where(f => f.MemberId1 == id).Select(f => f.MemberId).ToListAsync());
            var json = JsonSerializer.Serialize(friends);
            return json;
        }

        // PUT: api/Friend/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("accept")]
        public async Task<ActionResult<Friend>> PutFriend(Friend friend)
        {
            Friend f = _context.Friends.FirstOrDefault(f => (f.MemberId == friend.MemberId && f.MemberId1 == friend.MemberId1) || (f.MemberId1 == friend.MemberId && f.MemberId == friend.MemberId1));
            f.State = 1;
            _context.Entry(f).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return f;
        }

        // POST: api/Friend
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("add_friend")]
        public async Task<ActionResult<Friend>> AddFriend(Friend friend)
        {
            friend.State = 0;
            _context.Friends.Add(friend);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FriendExists(friend.MemberId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFriend", new { id = friend.MemberId }, friend);
        }

        // DELETE: api/Friend/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friend>> DeleteFriend(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();

            return friend;
        }

        private bool FriendExists(int id)
        {
            return _context.Friends.Any(e => e.MemberId == id);
        }
    }
}
