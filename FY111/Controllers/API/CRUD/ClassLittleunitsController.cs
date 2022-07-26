using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using FY111.Dtos;

namespace FY111.Controllers.API.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassLittleunitsController : ControllerBase
    {
        private readonly FY111Context _context;

        public ClassLittleunitsController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/ClassLittleunits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassLittleunit>>> GetClassLittleunits()
        {
            var list = await _context.ClassLittleunits.ToListAsync();
            return Ok(classLittleunitToDto(list));
        }

        // GET: api/ClassLittleunits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassLittleunitDto>> GetClassLittleunit(int id)
        {
            var classLittleunit = await _context.ClassLittleunits.FindAsync(id);

            if (classLittleunit == null)
            {
                return NotFound();
            }
            var dto = classLittleunitToDto(classLittleunit);
            return dto;
        }

        // PUT: api/ClassLittleunits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassLittleunit(int id, ClassLittleunit classLittleunit)
        {
            if (id != classLittleunit.Id)
                return BadRequest("Id 為PK，不可變更。");

            ClassLittleunit origin = await _context.ClassLittleunits.FindAsync(id);
            if (origin.ClassUnitId != classLittleunit.ClassUnitId)
                return BadRequest("與原課程父單元代號不符合，請重新嘗試或改用刪除並重建。");
            if (origin.Code != classLittleunit.Code)
            {
                bool check = await _context.ClassLittleunits
                    .Where(e => e.Code != origin.Code && e.ClassUnitId == classLittleunit.ClassUnitId && e.Code == classLittleunit.Code)
                    .AnyAsync();
                if (check)
                    return BadRequest("代號重複。");
            }
            if (origin.Name != classLittleunit.Name)
            {
                bool check = await _context.ClassLittleunits
                    .Where(e => e.Name != origin.Name && e.ClassUnitId == classLittleunit.ClassUnitId && e.Name == classLittleunit.Name)
                    .AnyAsync();
                if (check)
                    return BadRequest("名稱重複。");
            }
            // 通過檢查，嘗試執行修改
            try
            {
                _context.ChangeTracker.Clear();
                _context.Entry(classLittleunit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassLittleunitExists(id))
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

        // POST: api/ClassLittleunits
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassLittleunit>> PostClassLittleunit(ClassLittleunit classLittleunit)
        {
            bool? check = existClassUnit(classLittleunit.ClassUnitId);
            if (check == false || check == null)
                return BadRequest("查無父單元。");
            check = existCode(classLittleunit.Code);
            if (check == true || check == null)
                return BadRequest("子單元代號重複。");
            check = existName(classLittleunit.Name);
            if (check == true || check == null)
                return BadRequest("子單元名稱重複。");
            // 通過檢查，新增資料
            try
            {
                _context.ChangeTracker.Clear();
                _context.ClassLittleunits.Add(classLittleunit);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("資料庫新增失敗。");
            }
            return CreatedAtAction("GetClassLittleunit", new { id = classLittleunit.Id }, classLittleunit);
        }

        // DELETE: api/ClassLittleunits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClassLittleunit>> DeleteClassLittleunit(int id)
        {
            var classLittleunit = await _context.ClassLittleunits.FindAsync(id);
            if (classLittleunit == null)
            {
                return NotFound();
            }

            _context.ClassLittleunits.Remove(classLittleunit);
            await _context.SaveChangesAsync();

            return classLittleunit;
        }

        private bool ClassLittleunitExists(int id)
        {
            return _context.ClassLittleunits.Any(e => e.Id == id);
        }

        private ClassLittleunitDto classLittleunitToDto(ClassLittleunit classLittleunit)
        {
            ClassLittleunitDto dto = new ClassLittleunitDto();
            dto.Id = classLittleunit.Id;
            dto.ClassUnitId = classLittleunit.ClassUnitId;
            dto.Code = classLittleunit.Code;
            dto.Name = classLittleunit.Name;
            dto.Image = classLittleunit.Image;
            return dto;
        }

        private IEnumerable<ClassLittleunitDto> classLittleunitToDto(IEnumerable<ClassLittleunit> classLittleunits)
        {
            List<ClassLittleunitDto> result = new List<ClassLittleunitDto>();
            foreach (ClassLittleunit classLittleunit in classLittleunits)
            {
                ClassLittleunitDto dto = new ClassLittleunitDto();
                dto.Id = classLittleunit.Id;
                dto.ClassUnitId = classLittleunit.ClassUnitId;
                dto.Code = classLittleunit.Code;
                dto.Name = classLittleunit.Name;
                dto.Image = classLittleunit.Image;
                result.Add(dto);
            }
            return result;
        }

        private bool? existClassUnit(int Id)
        {
            try
            {
                var checkClassUnitId = _context.ClassUnits
                    .Where(e => e.Id == Id)
                    .Any();
                if (!checkClassUnitId)
                    return false;
            }
            catch
            {
                return null;
            }
            return true;
        }
        private bool? existCode(string code)
        {
            try
            {
                var checkClassUnitId = _context.ClassLittleunits
                    .Where(e => e.Code == code)
                    .Any();
                if (checkClassUnitId)
                    return true;
            }
            catch
            {
                return null;
            }
            return false;
        }

        private bool? existName(string name)
        {
            try
            {
                var checkClassUnitId = _context.ClassLittleunits
                    .Where(e => e.Name == name)
                    .Any();
                if (checkClassUnitId)
                    return true;
            }
            catch
            {
                return null;
            }
            return false;
        }
    }
}
