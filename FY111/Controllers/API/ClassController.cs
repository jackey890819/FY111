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
    [Route("api/")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public ClassController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("class")]
        public async Task<IActionResult> GetClass()
        {
            var @class = await _context.Classes.Select(x => new ClassDto(x)).ToListAsync();
            for (int i = 0; i < @class.Count; i++)
            {
                @class[i].units = await _context.ClassUnits.Where(x => x.ClassId == @class[i].@class.id).Select(x => new ClassUnitDto(x)).ToListAsync();
                for (int j=0; j<@class[i].units.Count; j++)
                {
                    @class[i].units.ElementAt(j).unit.littleUnits =
                        await _context.ClassLittleunits.Where(x => x.ClassUnitId == @class[i].units.ElementAt(j).unit.id)
                        .Select(x => new ClassLittleUnitDto(x)).ToListAsync();
                }
            }
            return Ok(new
            {
                data = @class
            });
        }

        [HttpPost("Enter")]
        public async Task<ActionResult<ClassLog>> EnterClass(ClassLog classLog)
        {
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new
                {
                    success = false,
                    message = "You are not logged in yet."
                });
            var user = await _userManager.GetUserAsync(User);
            var user_id = user.Id;
            //var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            classLog.MemberId = user_id;
            classLog.StartTime = DateTime.Now;
            _context.ClassLogs.Add(classLog);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Create Log Successfully"
            });
        }

        [HttpPatch("Leave")]
        public async Task<ActionResult<ClassLog>> LeaveClass()
        {
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new
                {
                    success = false,
                    message = "You are not logged in yet."
                });
            var user = await _userManager.GetUserAsync(User);
            var user_id = user.Id;
            //var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ClassLog classLog = _context.ClassLogs.FirstOrDefault(x => x.MemberId == user_id && x.EndTime == null);
            if (classLog == null)
                return BadRequest(new
                {
                    success = false,
                    message = "You haven't entered."
                });
            _context.Entry(classLog).State = EntityState.Modified;
            classLog.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Modify Log Successfully"
            });
        }

        [HttpGet("attendedList/{classCode}/{date}")]
        public async Task<IActionResult> GetAttendedList(string classCode, DateTime date)
        {
            int classId = (await _context.Classes.FirstOrDefaultAsync(x => x.Code == classCode)).Id;
            var signUpList = await _context.ClassSignups.Where(x => x.ClassId == classId).Select(x => x.MemberId).ToListAsync();
            var attendees = await _context.ClassLogs.Where(x => x.ClassId == classId && DateTime.Compare(x.StartTime, date) >= 0).Select(x => x.MemberId).ToListAsync();
            var logs = new List<AttendeeLogDto>();
            for (int i = 0; i < signUpList.Count; i++)
            {
                bool checkin = false;
                string name = (await _userManager.FindByIdAsync(signUpList[i])).UserName;
                if (attendees.Contains(signUpList[i])) checkin = true;
                var log = new AttendeeLogDto(signUpList[i], name, checkin);
                logs.Add(log);
            }
            return Ok(new
            {
                data = logs
            });
        }

        // GET: api/Class/{id}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetClass(int id)
        //{
        //    if (!_signInManager.IsSignedIn(User)) return BadRequest(new{
        //        errors = "Get class failed"
        //    });
        //    var @class = new ClassDto(await _context.Classes.SingleOrDefaultAsync(x => x.Id == id));
        //    if (@class == null)
        //        return NotFound(new
        //        {
        //            errors = $"id {id} not found"
        //        });
        //    var classUnits = await _context.ClassUnits.Where(u => u.ClassId == id).Select(u => new ClassUnitDto(u)).ToListAsync();
        //    for (int i = 0; i < classUnits.Count(); i++) 
        //    {
        //        classUnits[i].unit.littleUnits = await _context.ClassLittleunits.Where(u => u.ClassUnitId == classUnits[i].unit.id).Select(lu => new ClassLittleUnitDto(lu)).ToListAsync();
        //    }
        //    return Ok(new
        //    {
        //        data = new
        //        {
        //            @class,
        //            units = classUnits
        //        }
        //    });
        //if (User.IsInRole("SuperAdmin"))
        //{
        //    try
        //    {
        //        var result = await _context.Classes
        //            .Where(x => x.Id == id)
        //            .Select(x => new ClassDetailDto
        //            {
        //                Id = x.Id,
        //                Name = x.Name,
        //                Ip = x.Ip,
        //                Image = x.Image,
        //                Content = x.Content,
        //                SignupEnabled = x.SignupEnabled,
        //                CheckinEnabled = x.CheckinEnabled,
        //                Duration = x.Duration
        //            })
        //            .ToListAsync();
        //        if (!result.Any())
        //        {
        //            return NotFound(new
        //            {
        //                success = false,
        //                message = $"{id} not found. "
        //            });
        //        }
        //        return Ok(result[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}
        //else
        //{
        //    try
        //    {
        //        var result = await _context.Classes
        //            .Where(x => x.Id == id)
        //            .Select(x => new ClassDto
        //            {
        //                Id = x.Id,
        //                Name = x.Name,
        //                Content = x.Content,
        //                Duration = x.Duration
        //            })
        //            .ToListAsync();
        //        if (!result.Any())
        //        {
        //            return NotFound(new
        //            {
        //                success = false,
        //                message = $"{id} not found. "
        //            });
        //        }
        //        return Ok(result[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}
        //}


        // POST: api/Classes/create
        //[HttpPost("create")]
        //[Authorize(Roles = "SuperAdmin")]
        //public async Task<ActionResult> CreateMetaverse(ClassCreateModel model)
        //{
        //    // 檢查輸入
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "Input error."
        //        });
        //    }

        //    // 重複命名檢查
        //    var existed = await _context.Classes.Where(x => x.Name == model.Name).ToListAsync();
        //    if (existed.Any())
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "Class name existed."
        //        });
        //    // 檢查名稱無重複後，嘗試建立資料
        //    try
        //    {
        //        Class @class = new Class
        //        {
        //            Name = model.Name,
        //            Ip = model.Ip,
        //            Content = model.Content,
        //            SignupEnabled = model.SignupEnabled,
        //            CheckinEnabled = model.CheckinEnabled,
        //            Duration = model.Duration
        //        };
        //        _context.Classes.Add(@class);
        //        await _context.SaveChangesAsync();
        //        var id = await _context.Classes
        //                    .Where(x => x.Name == @class.Name)
        //                    .Select(x => x.Id)
        //                    .ToListAsync();
        //        ClassDetailDto result = new ClassDetailDto(@class);
        //        result.Id = id[0];
        //        return Ok(result);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            error = ex.Message
        //        });
        //    }
        //}


        // PUT: api/Class/update      
        //[HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult> UpdateClass(ClassUpdateModel model)
        //{
        //    // 檢查ModelState
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "Input error."
        //        });
        //    }
        //    var @class = await _context.Classes
        //        .Where(x => x.Id == model.Id)
        //        .ToListAsync();
        //    @class[0].Name = (model.Name == null) ? @class[0].Name : model.Name;
        //    @class[0].Ip = (model.Ip == null) ? @class[0].Ip : model.Ip;
        //    @class[0].Image = (model.Image == null) ? @class[0].Image : model.Image;
        //    @class[0].Content = (model.Content == null) ? @class[0].Content : model.Name;
        //    @class[0].SignupEnabled = (byte)((model.SignupEnabled == null) ? @class[0].SignupEnabled : model.SignupEnabled);
        //    @class[0].CheckinEnabled = (byte)((model.CheckinEnabled == null) ? @class[0].CheckinEnabled : model.CheckinEnabled);
        //    @class[0].Duration = (model.Duration == null) ? @class[0].Duration : model.Duration;

        //    try
        //    {
        //        _context.Entry(@class[0]).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Update successful."
        //    });
        //}



        // DELETE: api/Classes/delete/{id}
        // 依據id刪除元宇宙
        //[HttpDelete("delete/{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //public async Task<ActionResult<Class>> DeleteClass(int id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "Input error."
        //        });

        //    var @class = await _context.Classes.FindAsync(id);
        //    if (@class == null)
        //    {
        //        return NotFound(new
        //        {
        //            success = false,
        //            message = "Class not found."
        //        });
        //    }
        //    try
        //    {
        //        _context.Classes.Remove(@class);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Delete successful."
        //    });
        //}

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

        private bool ClassExists(int id)
    {
        return _context.Classes.Any(e => e.Id == id);
    }
}
}
