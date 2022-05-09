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
using FY111.Models.Dto;

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

        // GET: api/Metaverse/id/{id}
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetMetaverse(int id)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                try
                {
                    var result = await _context.Metaverses
                        .Where(x => x.Id == id)
                        .Select(x => new MetaverseDetailDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Ip = x.Ip,
                            Icon = x.Icon,
                            Introduction = x.Introduction,
                            SignupEnabled = x.SignupEnabled,
                            CheckinEnabled = x.CheckinEnabled,
                            Duration = x.Duration
                        })
                        .ToListAsync();
                    if (!result.Any())
                    {
                        return NotFound(new
                        {
                            success = false,
                            message = $"{id} not found. "
                        });
                    }
                    return Ok(result[0]);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }
            else
            {
                try
                {
                    var result = await _context.Metaverses
                        .Where(x => x.Id == id)
                        .Select(x => new MetaverseDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Introduction = x.Introduction,
                            Duration = x.Duration
                        })
                        .ToListAsync();
                    if (!result.Any())
                    {
                        return NotFound(new
                        {
                            success = false,
                            message = $"{id} not found. "
                        });
                    }
                    return Ok(result[0]);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }
            
        }


        // POST: api/Metaverses/create
        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> CreateMetaverse(MetaverseCreateModel model)
        {
            // 檢查輸入
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Input error."
                });
            }
                
            // 重複命名檢查
            var existed = await _context.Metaverses.Where(x => x.Name == model.Name).ToListAsync();
            if (existed.Any())
                return BadRequest(new
                {
                    success = false,
                    message = "Metaverse name existed."
                });
            // 檢查名稱無重複後，嘗試建立資料
            try
            {
                Metaverse metaverse = new Metaverse{ 
                    Name = model.Name,
                    Ip = model.Ip,
                    Introduction = model.Introduction,
                    SignupEnabled = model.SignupEnabled,
                    CheckinEnabled = model.CheckinEnabled,
                    Duration = model.Duration
                };
                _context.Metaverses.Add(metaverse);
                await _context.SaveChangesAsync();
                var id = await _context.Metaverses
                            .Where(x => x.Name == metaverse.Name)
                            .Select(x => x.Id)
                            .ToListAsync();
                MetaverseDetailDto result = new MetaverseDetailDto(metaverse);
                result.Id = id[0];
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new {
                    success = false,
                    error = ex.Message
                });
            }
        }


        // PUT: api/Metaverses/update      
        [HttpPut("update")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateMetaverse(MetaverseUpdateModel model)
        {
            // 檢查ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Input error."
                });
            }
            var metaverse = await _context.Metaverses
                .Where(x=>x.Id == model.Id)
                .ToListAsync();
            metaverse[0].Name = (model.Name == null) ? metaverse[0].Name : model.Name;
            metaverse[0].Ip = (model.Ip == null) ? metaverse[0].Ip : model.Ip;
            metaverse[0].Icon = (model.Icon == null) ? metaverse[0].Icon : model.Icon;
            metaverse[0].Introduction = (model.Introduction == null) ? metaverse[0].Introduction : model.Name;
            metaverse[0].SignupEnabled = (byte)((model.SignupEnabled == null) ? metaverse[0].SignupEnabled : model.SignupEnabled);
            metaverse[0].CheckinEnabled = (byte)((model.CheckinEnabled == null) ? metaverse[0].CheckinEnabled : model.CheckinEnabled);
            metaverse[0].Duration = (model.Duration == null) ? metaverse[0].Duration : model.Duration;

            try
            {
                _context.Entry(metaverse[0]).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            return Ok(new
            {
                success = true,
                message = "Update successful."
            });
        }

        

        // DELETE: api/Metaverses/delete/{id}
        // 依據id刪除元宇宙
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Metaverse>> DeleteMetaverse(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new 
                { 
                    success = false,
                    message = "Input error."
                });

            var metaverse = await _context.Metaverses.FindAsync(id);
            if (metaverse == null)
            {
                return NotFound(new {
                    success = false,
                    message = "Metaverse not found."
                });
            }
            try
            {
                _context.Metaverses.Remove(metaverse);
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            return Ok(new
            {
                success = true,
                message = "Delete successful."
            });
        }

        //[HttpGet("signup_enabled")]
        //[Authorize(Roles = "NormalUser, GroupUser, MetaverseAdmin, SuperAdmin")]
        //public async Task<ActionResult<IEnumerable<Object>>> ListAvailable() //ListAvailable_Model model
        //{
        //    // 確認使用者的權限類型
        //    if (User.IsInRole("NormalUser") || User.IsInRole("GroupUser"))
        //    {
        //        // 若為一般使用者或團體使用者：分列出已報名及未報名的且可報名的元宇宙List
        //        var userId = _userManager.GetUserId(User);
        //        var selected_list = await _context.MetaverseSignups
        //                            .Where(x => x.MemberId == userId)
        //                            .Select(x => x.MetaverseId).ToListAsync();

        //        var selected_metaverse = await _context.Metaverses
        //                                .Where(x => selected_list.Contains(x.Id)).ToListAsync();

        //        var metaverse = await _context.Metaverses
        //                        .Where(b => b.SignupEnabled == 1 && !selected_metaverse.Contains(b)) // 列出尚未選擇並且可選擇的元宇宙
        //                        .ToListAsync();

        //        if (metaverse == null)
        //        {
        //            return NotFound();
        //        }
        //        return new[]
        //        {
        //            metaverse,
        //            selected_metaverse
        //        };
        //    }
        //    else if (User.IsInRole("MetaverseAdmin") || User.IsInRole("SuperAdmin"))
        //    {
        //        // 管理員：列出所有的元宇宙
        //        var metaverseList = await _context.Metaverses.ToListAsync();

        //        return metaverseList;
        //    }
        //    return BadRequest();
        //}

        [HttpPost("test")]
        public async Task<ActionResult> Test(MetaverseUpdateModel model)
        {
            return Ok(model);
        }

        private bool MetaverseExists(int id)
        {
            return _context.Metaverses.Any(e => e.Id == id);
        }
    }
}
