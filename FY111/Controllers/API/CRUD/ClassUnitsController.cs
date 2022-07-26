using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Authorization;
using FY111.Dtos;

namespace FY111.Controllers.API.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUnitsController : ControllerBase
    {
        private readonly FY111Context _context;

        public ClassUnitsController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/ClassUnits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassUnitCRUDDto>>> GetClassUnits()
        {
            return await _context.ClassUnits
                .Select(e => new ClassUnitCRUDDto
                {
                    Id = e.Id,
                    ClassId = e.ClassId,
                    Code = e.Code,
                    Name = e.Name,
                    Image = e.Image
                })
                .ToListAsync();
        }

        // GET: api/ClassUnits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassUnitCRUDDto>> GetClassUnit(int id)
        {
            var classUnit = await _context.ClassUnits
                .Where(e => e.Id == id)
                .Select(e => new ClassUnitCRUDDto
                {
                    Id = e.Id,
                    ClassId = e.ClassId,
                    Code = e.Code,
                    Name = e.Name,
                    Image = e.Image
                }).SingleOrDefaultAsync();

            if (classUnit == null)
            {
                return NotFound("找不到該id之單元。");
            }

            return classUnit;
        }

        // PUT: api/ClassUnits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<IActionResult> PutClassUnit(int id, ClassUnit classUnit)
        {
            if (id != classUnit.Id)
                return BadRequest("Id 為PK，不可變更。");
            // 取原值，後續檢驗用
            ClassUnit classUnitOrigin = _context.ClassUnits.FirstOrDefault(e => e.Id == id);
            // 檢查是否屬於原課程之單元
            if (classUnitOrigin.ClassId != classUnit.ClassId)
                return BadRequest("與原課程代號不符合，請重新嘗試或改用刪除並重建。");
            // 檢察同課程中是否有重複的代號
            if (classUnitOrigin.Code != classUnit.Code)
            {
                var check = _context.ClassUnits.Where(c => c.ClassId == classUnit.ClassId && c.Code == classUnit.Code && c.Code != classUnitOrigin.Code).Any();
                if (check)
                    return BadRequest("新單元代號重複。");
            }
            // 檢察同課程中是否有重複的名稱
            if (classUnitOrigin.Name != classUnit.Name)
            {
                var check = _context.ClassUnits.Where(c => c.ClassId == classUnit.ClassId && c.Name == classUnit.Name && c.Name != classUnitOrigin.Name).Any();
                if (check)
                    return BadRequest("新單元名稱重複。");
            }

            // 通過檢查，執行修改
            _context.ChangeTracker.Clear();
            _context.Entry(classUnit).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassUnitExists(id))
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

        // POST: api/ClassUnits
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<ActionResult<ClassUnit>> PostClassUnit(ClassUnit classUnit)
        {
            // 查詢是否有對應的課程
            try
            {
                var checkClassId = await _context.Classes
                    .Where(e => e.Id == classUnit.ClassId)
                    .AnyAsync();
                if (!checkClassId)
                    return BadRequest("無對應的課程。");
            }
            catch
            {
                return BadRequest("檢查過程中發生資料庫錯誤。");
            }

            // 檢察同課程中是否有重複的代號
            try
            {
                var checkCode = await _context.ClassUnits
                    .Where(e => e.Code == classUnit.Code)
                    .AnyAsync();
                if (checkCode)
                    return BadRequest("新單元代號重複。");
            }
            catch
            {
                return BadRequest("檢查過程中發生資料庫錯誤。");
            }

            // 檢察同課程中是否有重複的名稱
            try
            {
                var checkName = await _context.ClassUnits
                    .Where(e => e.Name == classUnit.Name)
                    .AnyAsync();
                if (checkName)
                    return BadRequest("新單元名稱重複。");
            }
            catch
            {
                return BadRequest("檢查過程中發生資料庫錯誤。");
            }

            // 無誤，新增資料
            _context.ClassUnits.Add(classUnit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassUnit", new { id = classUnit.Id }, classUnit);
        }

        // DELETE: api/ClassUnits/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin, ClassAdmin")]
        public async Task<ActionResult<ClassUnit>> DeleteClassUnit(int id)
        {
            var classUnit = await _context.ClassUnits.FindAsync(id);
            if (classUnit == null)
            {
                return NotFound();
            }

            _context.ClassUnits.Remove(classUnit);
            await _context.SaveChangesAsync();

            return classUnit;
        }

        private bool ClassUnitExists(int id)
        {
            return _context.ClassUnits.Any(e => e.Id == id);
        }
    }
}
