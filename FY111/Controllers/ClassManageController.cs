using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Authorize(Roles = "SuperAdmin, MetaverseAdmin")]
    public class ClassManageController : Controller
    {
        private readonly FY111Context _context;

        public ClassManageController(FY111Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["SignUpParm"] = String.IsNullOrEmpty(sortOrder) ? "signup_desc" : "";
            ViewData["CheckInParm"] = sortOrder == "checkin" ? "checkin_desc" : "checkin";
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            IQueryable<Class> classes;
            if (!String.IsNullOrEmpty(searchString))
            {
                classes = _context.Classes.Where(s => s.Name.Contains(searchString)
                                       || s.Content.Contains(searchString)).Select(x => x);
            }
            else classes = _context.Classes.Select(x => x);

            foreach(Class c in classes.ToList())
            {
                c.ClassUnits = await _context.ClassUnits.Where(x => x.ClassId == c.Id).ToListAsync();
            }

            switch (sortOrder)
            {
                case "signup_desc":
                    classes = classes.OrderByDescending(x => x.SignupEnabled);
                    break;
                case "checkin":
                    classes = classes.OrderBy(x => x.CheckinEnabled);
                    break;
                case "checkin_desc":
                    classes = classes.OrderByDescending(x => x.CheckinEnabled);
                    break;
                default:
                    classes = classes.OrderBy(x => x.SignupEnabled);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Class>.CreateAsync(classes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: ClassManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // GET: ClassManage/DetailsUnit/5
        public async Task<IActionResult> DetailsUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var unit = await _context.ClassUnits
                .FirstOrDefaultAsync(x => x.Id == id);
            unit.ClassLittleunits = await _context.ClassLittleunits.Where(x => x.ClassUnitId == id).ToListAsync();
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: ClassManage/DetailsLittleUnit/5
        public async Task<IActionResult> DetailsLittleUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var little = await _context.ClassLittleunits
                .FirstOrDefaultAsync(x => x.Id == id);
            //little.ClassLittleunits = await _context.ClassLittleunits.Where(x => x.ClassUnitId == id).ToListAsync();
            if (little == null)
            {
                return NotFound();
            }

            return View(little);
        }

        // GET: ClassManage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Ip,Image,Content,SignupEnabled,CheckinEnabled,Duration")] Class classes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classes);
        }

        // GET: ClassManage/CreateUnit
        public IActionResult CreateUnit(int ClassId, string ClassCode)
        {
            ViewData["ClassId"] = ClassId;
            ViewData["ClassCode"] = ClassCode;
            return View();
        }

        // POST: ClassManage/CreateUnit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUnit([Bind("Id,ClassId,Code,Name,Image")] ClassUnit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = unit.ClassId });
            }
            return View(unit);
        }

        // GET: ClassManage/CreateLittleUnit
        public IActionResult CreateLittleUnit(int ClassUnitId, string ClassUnitCode)
        {
            ViewData["ClassUnitId"] = ClassUnitId;
            ViewData["ClassUnitCode"] = ClassUnitCode;
            return View();
        }
        // POST: ClassManage/CreateLittleUnit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLittleUnit([Bind("Id,ClassUnitId,Code,Name,Image")] ClassLittleunit little)
        {
            if (ModelState.IsValid)
            {
                _context.Add(little);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsUnit), new { id = little.ClassUnitId });
            }
            return View(little);
        }

        // GET: ClassManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return View(@class);
        }

        // POST: ClassManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Ip,Image,Content,SignupEnabled,CheckinEnabled,Duration")] Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (@class.Image == null)
                    {
                        string result = (await _context.Classes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Image;
                        @class.Image = result;
                    }
                    else
                    {
                        var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\Class\\");
                        var imageName = (await _context.Classes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Image;
                        //System.IO.File.Delete(dirPath+imageName);
                    }
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@class);
        }

        // GET: ClassManage/EditUnit/5
        public async Task<IActionResult> EditUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.ClassUnits.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
            {
                return NotFound();
            }
            return View(unit);
        }

        // POST: ClassManage/EditUnit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUnit(int id, [Bind("Id,ClassId,Code,Name,Image")] ClassUnit unit)
        {
            if (id != unit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string result = (await _context.ClassUnits.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Image;
                    if (unit.Image == null)
                    {
                        unit.Image = result;
                    }
                    else
                    {
                        var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\ClassUnit\\");
                        //System.IO.File.Delete(dirPath + result);
                    }
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassUnitExists(unit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = unit.ClassId });
            }
            return View(unit);
        }

        // GET: ClassManage/EditLittleUnit/5
        public async Task<IActionResult> EditLittleUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var little = await _context.ClassLittleunits.FirstOrDefaultAsync(x => x.Id == id);
            if (little == null)
            {
                return NotFound();
            }
            return View(little);
        }
        // POST: ClassManage/EditLittleUnit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLittleUnit(int id, [Bind("Id,ClassUnitId,Code,Name,Image")] ClassLittleunit little)
        {
            if (id != little.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string result = (await _context.ClassLittleunits.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Image;
                    if (little.Image == null)
                    {
                        little.Image = result;
                    }
                    else
                    {
                        var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\ClassLittleUnit\\");
                        //System.IO.File.Delete(dirPath + result);
                    }
                    _context.Update(little);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassLittleUnitExists(little.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DetailsUnit), new { id = little.ClassUnitId });
            }
            return View(little);
        }

        // GET: ClassManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // GET: ClassManage/DeleteUnit/5
        public async Task<IActionResult> DeleteUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.ClassUnits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: ClassManage/DeleteLittleUnit/5
        public async Task<IActionResult> DeleteLittleUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var little = await _context.ClassLittleunits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (little == null)
            {
                return NotFound();
            }

            return View(little);
        }

        // POST: ClassManage/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\Class\\");
            var imageName = @class.Image;
            System.IO.File.Delete(dirPath + imageName);
            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ClassManage/DeleteUnitConfirmed/5
        [HttpPost, ActionName("DeleteUnit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUnitConfirmed(int id)
        {
            var unit = await _context.ClassUnits.FirstOrDefaultAsync(m => m.Id == id);
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\ClassUnit\\");
            var imageName = unit.Image;
            System.IO.File.Delete(dirPath + imageName);
            _context.ClassUnits.Remove(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = unit.ClassId });
        }

        // POST: ClassManage/DeleteLittleUnitConfirmed/5
        [HttpPost, ActionName("DeleteLittleUnit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLittleUnitConfirmed(int id)
        {
            var little = await _context.ClassLittleunits.FirstOrDefaultAsync(m => m.Id == id);
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\ClassLittleUnit\\");
            var imageName = little.Image;
            System.IO.File.Delete(dirPath + imageName);
            _context.ClassLittleunits.Remove(little);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsUnit), new { id = little.ClassUnitId });
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
        private bool ClassUnitExists(int id)
        {
            return _context.ClassUnits.Any(e => e.Id == id);
        }
        private bool ClassLittleUnitExists(int id)
        {
            return _context.ClassLittleunits.Any(e => e.Id == id);
        }
    }
}
