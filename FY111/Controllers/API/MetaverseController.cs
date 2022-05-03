using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Identity;
using FY111.Areas.Identity.Data;
using FY111.Models;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public MetaverseController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // PUT: api/Metaverses/5      
        [HttpPatch("edit/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditMetaverse(int id, Metaverse metaverse)
        {
            if (id != metaverse.Id)
            {
                return BadRequest();
            }

            _context.Entry(metaverse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetaverseExists(id))
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

        // POST: api/Metaverses
        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Metaverse>> PostMetaverse(Metaverse metaverse)
        {
            _context.Metaverses.Add(metaverse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MetaverseExists(metaverse.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMetaverse", new { id = metaverse.Name }, metaverse);
        }

        // DELETE: api/Metaverses/id
        // 依據id刪除元宇宙
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Metaverse>> RemoveMetaverse(int id)
        {
            var metaverse = await _context.Metaverses.FindAsync(id);
            if (metaverse == null)
            {
                return NotFound();
            }
            _context.Metaverses.Remove(metaverse);
            await _context.SaveChangesAsync();
            return metaverse;
        }

        [HttpGet("list_available")]
        [Authorize(Roles = "NormalUser, GroupUser, MetaverseAdmin, SuperAdmin")]
        public async Task<ActionResult<IEnumerable<Object>>> ListAvailable() //ListAvailable_Model model
        {
            // 確認使用者的權限類型
            if (User.IsInRole("NormalUser") || User.IsInRole("GroupUser"))
            {
                // 若為一般使用者或團體使用者：分列出已報名及未報名的且可報名的元宇宙List
                var userId = _userManager.GetUserId(User);
                var selected_list = await _context.MetaverseSignIns
                                    .Where(x => x.MemberId == userId)
                                    .Select(x => x.MetaverseId).ToListAsync();

                var selected_metaverse = await _context.Metaverses
                                        .Where(x => selected_list.Contains(x.Id)).ToListAsync();

                var metaverse = await _context.Metaverses
                                .Where(b => b.SigninEnabled == 1 && !selected_metaverse.Contains(b)) // 列出尚未選擇並且可選擇的元宇宙
                                .ToListAsync();

                if (metaverse == null)
                {
                    return NotFound();
                }
                return new[]
                {
                    metaverse,
                    selected_metaverse
                };
            }
            else if (User.IsInRole("MetaverseAdmin") || User.IsInRole("SuperAdmin"))
            {
                // 管理員：列出所有的元宇宙
                var metaverseList = await _context.Metaverses.ToListAsync();

                return metaverseList;
            }
            return BadRequest();
        }

        private bool MetaverseExists(int id)
        {
            return _context.Metaverses.Any(e => e.Id == id);
        }
    }
}
