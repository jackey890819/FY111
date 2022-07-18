using FY111.Areas.Identity.Data;
using FY111.Dtos;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public OperationController(FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 更新操作紀錄
        // POST: /api/operationRecord
        [HttpPost("operationRecord")]
        public async Task<IActionResult> SetOperationRecord(OperationUnitLogDto operationUnitLogDto)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 更新紀錄
            try
            {
                OperationUnitLog log = new OperationUnitLog();
                log.UnitCode = operationUnitLogDto.unit_code;
                log.MemberId = _userManager.GetUserId(User);
                log.Pass = 0;
                _context.OperationUnitLogs.Add(log);
                await _context.SaveChangesAsync();
                int operationId = _context.OperationUnitLogs.OrderBy(x => x.Id)
                        .LastOrDefault(x => x.MemberId == _userManager.GetUserId(User) && x.UnitCode == operationUnitLogDto.unit_code).Id;
                int classUnitId = (await _context.ClassUnits.FirstOrDefaultAsync(x => x.Code == operationUnitLogDto.unit_code)).Id;
                List<string> unitCodes = await _context.ClassLittleunits.Where(x => x.ClassUnitId == classUnitId).Select(x => x.Code).ToListAsync();
                for (int i = 0; i < unitCodes.Count; i++)
                {
                    OperationLittleunitLog littleUnitLog = new OperationLittleunitLog();
                    littleUnitLog.OperationLogId = operationId;
                    littleUnitLog.LittleunitCode = unitCodes[i];
                    littleUnitLog.Score = 0;
                    littleUnitLog.Pass = 0;
                    _context.OperationLittleunitLogs.Add(littleUnitLog);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { success = "Create successfully" });
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Record failed" });
            }
        }


        // 紀錄使用者操作階段
        // POST: /api/operationCheckpoint
        [HttpPost("operationCheckpoint")]
        public async Task<IActionResult> SetOperationCheckpoint(OperationCheckPointDto operationCheckPointDto)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 紀錄
            try
            {
                int operationId = _context.OperationUnitLogs.OrderBy(x => x.Id)
                    .LastOrDefault(x => x.MemberId == _userManager.GetUserId(User) && x.UnitCode == operationCheckPointDto.unit_code).Id;
                for (int i = 0; i < operationCheckPointDto.little_unit.Count; i++)
                {
                    int operationLittleUnitId = _context.OperationLittleunitLogs.OrderBy(x => x.Id)
                        .LastOrDefault(x => x.OperationLogId == operationId && x.LittleunitCode == operationCheckPointDto.little_unit.ElementAt(i).code).Id;
                    for (int j = 0; j < operationCheckPointDto.little_unit.ElementAt(i).CheckPoint.Count; j++)
                    {
                        OperationCheckpoint ckpt = new OperationCheckpoint();
                        ckpt.OperationLittleunitLogId = operationLittleUnitId;
                        ckpt.CkptId = operationCheckPointDto.little_unit.ElementAt(i).CheckPoint.ElementAt(j).CKPT_id;
                        ckpt.PointType = operationCheckPointDto.little_unit.ElementAt(i).CheckPoint.ElementAt(j).PointType;
                        _context.OperationCheckpoints.Add(ckpt);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new { success = "Create successfully" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Record failed" });
            }
        }


        // 取得使用者操作階段
        // GET: /api/operationCheckpoint/{大單元代碼}
        [HttpGet("operationCheckpoint/{unitCode}")]
        public async Task<IActionResult> GetOperationCheckpoint(string unitCode)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });

            try
            {
                int operationId = _context.OperationUnitLogs.OrderBy(x => x.Id)
                      .LastOrDefault(x => x.MemberId == _userManager.GetUserId(User) && x.UnitCode == unitCode).Id;
                List<OperationLittleunitLog> operationLittleUnits = await _context.OperationLittleunitLogs
                    .Where(x => x.OperationLogId == operationId).ToListAsync();
                // 查詢不到時
                if (!operationLittleUnits.Any()) throw new Exception("not found");
                List<OperationLittleUnitCheckPoinDto_2> logs = new List<OperationLittleUnitCheckPoinDto_2>();
                for (int i = 0; i < operationLittleUnits.Count; i++)
                {
                    OperationLittleUnitCheckPoinDto_2 log = new OperationLittleUnitCheckPoinDto_2();
                    log.little_unit_code = operationLittleUnits[i].LittleunitCode;
                    log.CheckPoint = await _context.OperationCheckpoints
                        .Where(x => x.OperationLittleunitLogId == operationLittleUnits[i].Id).Select(x => new CheckPointDto(x)).ToListAsync();
                    logs.Add(log);
                }
                return Ok(new { data = logs });
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Get record failed" });
            }
        }


        // 上傳職災資訊
        // POST: /api/operationDisaster
        [HttpPost("operationDisaster")]
        public async Task<IActionResult> SetOperationDisaster(OperationDisasterDto operationDisasterDto)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 保存資訊
            try
            {
                int operationId = _context.OperationUnitLogs.OrderBy(x => x.Id)
                    .LastOrDefault(x => x.MemberId == _userManager.GetUserId(User) && x.UnitCode == operationDisasterDto.unit_code).Id;
                for (int i = 0; i < operationDisasterDto.little_unit.Count; i++)
                {
                    int operationLittleUnitId = (await _context.OperationLittleunitLogs.OrderBy(x => x.Id)
                            .LastOrDefaultAsync(x => x.OperationLogId == operationId && x.LittleunitCode == operationDisasterDto.little_unit.ElementAt(i).code)).Id;
                    for (int j = 0; j < operationDisasterDto.little_unit.ElementAt(i).OccDisaster.Count; j++)
                    {
                        OperationOccdisaster disaster = new OperationOccdisaster();
                        disaster.OperationLittleunitLogId = operationLittleUnitId;
                        disaster.OccDisasterCode = operationDisasterDto.little_unit.ElementAt(i).OccDisaster.ElementAt(j);
                        _context.OperationOccdisasters.Add(disaster);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new { success = "Create successfully" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Record failed" });
            }
        }


        // 取得職災資訊
        // GET: /api/operationDisaster/{大單元代碼}
        [HttpGet("operationDisaster/{unitCode}")]
        public async Task<IActionResult> GetOperationDisaster(string unitCode)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });

            try
            {
                List<int> operationids = await _context.OperationUnitLogs
                .Where(x => x.MemberId == _userManager.GetUserId(User) && x.UnitCode == unitCode).Select(x => x.Id).ToListAsync();
                List<string> littleUnitCodes = await _context.OperationLittleunitLogs
                    .Where(x => x.OperationLogId == operationids[0]).Select(x => x.LittleunitCode).ToListAsync();
                int littleUnitCount = _context.OperationLittleunitLogs.Where(x => x.OperationLogId == operationids[0]).Count();
                OperationDisasterDto_2 log = new OperationDisasterDto_2();
                log.unit_code = unitCode;
                for (int i = 0; i < littleUnitCodes.Count; i++)
                {
                    OperationLittleUnitDisasterDto_2 littleUnitDisaster = new OperationLittleUnitDisasterDto_2();
                    littleUnitDisaster.little_unit_code = littleUnitCodes[i];
                    int newestid = (await _context.OperationLittleunitLogs.OrderBy(x => x.Id)
                        .LastOrDefaultAsync(x => operationids.Contains(x.OperationLogId) && x.LittleunitCode == littleUnitCodes[i])).Id;
                    int highestid = (await _context.OperationLittleunitLogs.OrderBy(x => x.Score)
                        .LastOrDefaultAsync(x => operationids.Contains(x.OperationLogId) && x.LittleunitCode == littleUnitCodes[i])).Id;
                    List<string> newestDisasterCodes = await _context.OperationOccdisasters
                        .Where(x => x.OperationLittleunitLogId == newestid).Select(x => x.OccDisasterCode).ToListAsync();
                    List<string> highestDisasterCodes = await _context.OperationOccdisasters
                        .Where(x => x.OperationLittleunitLogId == highestid).Select(x => x.OccDisasterCode).ToListAsync();
                    for (int j = 0; j < newestDisasterCodes.Count; j++)
                    {
                        littleUnitDisaster.ScoreListData.NewestData
                            .Add(new OccdisasterDto(await _context.Occdisasters.FirstOrDefaultAsync(x => x.Code == newestDisasterCodes[j])));
                    }
                    for (int j = 0; j < highestDisasterCodes.Count; j++)
                    {
                        littleUnitDisaster.ScoreListData.HighestData
                            .Add(new OccdisasterDto(await _context.Occdisasters.FirstOrDefaultAsync(x => x.Code == highestDisasterCodes[j])));
                    }
                    log.little_unit.Add(littleUnitDisaster);
                }
                return Ok(new { data = log });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Get record failed" });
            }


            
        }


        // 上傳學習成績
        // POST: /api/operationResult
        [HttpPost("operationResult")]
        public async Task<IActionResult> SetOperationResult(OperationResultDto operationResult)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 儲存成績
            try
            {
                int operationId = (await _context.OperationUnitLogs.OrderBy(x => x.Id)
                .LastOrDefaultAsync(x => x.UnitCode == operationResult.unit_code && x.MemberId == _userManager.GetUserId(User))).Id;
                for (int i = 0; i < operationResult.little_unit.Count; i++)
                {
                    OperationLittleunitLog littleunitLog = await _context.OperationLittleunitLogs.OrderBy(x => x.Id)
                        .LastOrDefaultAsync(x => x.OperationLogId == operationId && x.LittleunitCode == operationResult.little_unit.ElementAt(i).code);
                    littleunitLog.Score = operationResult.little_unit.ElementAt(i).ScoreData.score;
                    littleunitLog.Pass = operationResult.little_unit.ElementAt(i).ScoreData.pass == "True" ? (byte)1 : (byte)0;
                    littleunitLog.StartTime = operationResult.little_unit.ElementAt(i).ScoreData.StartDateTime;
                    littleunitLog.EndTime = operationResult.little_unit.ElementAt(i).ScoreData.EndDateTime;
                    _context.Update(littleunitLog);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { success = "Update successfully" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Record failed" });
            }
        }


        // 取得當下受測成績
        // GET: /api/operationResult/{大單元代碼}
        [HttpGet("operationResult/{unitCode}")]
        public async Task<IActionResult> GetOperationResult(string unitCode)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 嘗試取值
            try
            {
                OperationResultDto_2 result = new OperationResultDto_2();
                OperationUnitLog unitLog = await _context.OperationUnitLogs.OrderBy(x => x.Id)
                    .LastOrDefaultAsync(x => x.UnitCode == unitCode && x.MemberId == _userManager.GetUserId(User));
                result.unit_code = unitCode;
                result.unit_pass = unitLog.Pass == 1 ? true : false;
                List<OperationLittleunitLog> littleUnitLogs = await _context.OperationLittleunitLogs.Where(x => x.OperationLogId == unitLog.Id).ToListAsync();
                for (int i = 0; i < littleUnitLogs.Count; i++)
                {
                    OperationLittleUnitResultDto_2 littleUnitLog = new OperationLittleUnitResultDto_2();
                    littleUnitLog.little_unit_code = littleUnitLogs[i].LittleunitCode;
                    littleUnitLog.ScoreData.score = (int)littleUnitLogs[i].Score;
                    littleUnitLog.ScoreData.pass = littleUnitLogs[i].Pass == 1 ? true : false;
                    littleUnitLog.ScoreData.StartDateTime = littleUnitLogs[i].StartTime;
                    littleUnitLog.ScoreData.EndDateTime = littleUnitLogs[i].EndTime;
                    littleUnitLog.ScoreData.TestTimeLast = littleUnitLog.ScoreData.EndDateTime - littleUnitLog.ScoreData.StartDateTime;
                    result.ScoreState.Add(littleUnitLog);
                }
                return Ok(new { data = result });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Get record failed" });
            }
        }


        // 取得上次受測成績
        // GET: /api/lastScore/{課程代碼}
        [HttpGet("lastScore/{classCode}")]
        public async Task<IActionResult> GetLastScore(string classCode)
        {
            // 檢查授權
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            // 嘗試取得成績
            try
            {
                int classId = (await _context.Classes.FirstOrDefaultAsync(x => x.Code == classCode)).Id;
                List<ClassUnit> units = await _context.ClassUnits.Where(x => x.ClassId == classId).ToListAsync();
                List<OperationResultDto_3> results = new List<OperationResultDto_3>();
                for (int i = 0; i < units.Count; i++)
                {
                    OperationResultDto_3 result = new OperationResultDto_3();
                    result.unit_code = units[i].Code;
                    List<int> operationids = await _context.OperationUnitLogs
                        .Where(x => x.UnitCode == units[i].Code && x.MemberId == _userManager.GetUserId(User)).Select(x => x.Id).ToListAsync();
                    List<string> littleunitCodes = await _context.ClassLittleunits.Where(x => x.ClassUnitId == units[i].Id).Select(x => x.Code).ToListAsync();
                    for (int j = 0; j < littleunitCodes.Count; j++)
                    {
                        OperationLittleUnitResultDto_3 littleunitResult = new OperationLittleUnitResultDto_3();
                        littleunitResult.little_unit_code = littleunitCodes[j];
                        if (operationids.Count > 0)
                        {
                            OperationLittleunitLog newestLogs = await _context.OperationLittleunitLogs
                               .Where(x => operationids.Contains(x.OperationLogId) && x.LittleunitCode == littleunitCodes[j]).OrderBy(x => x.Id).LastOrDefaultAsync();
                            OperationLittleunitLog highestLogs = await _context.OperationLittleunitLogs
                                .Where(x => operationids.Contains(x.OperationLogId) && x.LittleunitCode == littleunitCodes[j]).OrderBy(x => x.Score).LastOrDefaultAsync();
                            littleunitResult.ScoreListData.NewestData.score = (int)newestLogs.Score;
                            littleunitResult.ScoreListData.NewestData.pass = newestLogs.Pass == 1 ? true : false;
                            littleunitResult.ScoreListData.NewestData.StartDateTime = newestLogs.StartTime;
                            littleunitResult.ScoreListData.NewestData.EndDateTime = newestLogs.EndTime;
                            littleunitResult.ScoreListData.NewestData.TestTimeLast = newestLogs.EndTime - newestLogs.StartTime;

                            littleunitResult.ScoreListData.HighestData.score = (int)highestLogs.Score;
                            littleunitResult.ScoreListData.HighestData.pass = highestLogs.Pass == 1 ? true : false;
                            littleunitResult.ScoreListData.HighestData.StartDateTime = highestLogs.StartTime;
                            littleunitResult.ScoreListData.HighestData.EndDateTime = highestLogs.EndTime;
                            littleunitResult.ScoreListData.HighestData.TestTimeLast = highestLogs.EndTime - highestLogs.StartTime;
                        }
                        result.little_unit.Add(littleunitResult);
                    }
                    results.Add(result);
                }
                return Ok(new { data = results });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "Get record failed" });
            }
        }

        
    }
}
