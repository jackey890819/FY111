using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Authorize(Roles = "SuperAdmin, ClassAdmin")]
    public class ClassUnitCkptsManageController : Controller
    {
        private readonly FY111Context _context;

        public ClassUnitCkptsManageController(FY111Context context)
        {
            _context = context;
        }

        // GET: ClassUnitCkptsManage
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null) {
                pageNumber = 1;
            }
            else {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var classunitckpts = _context.ClassUnitCkpts.Select(x => x);
            if (!String.IsNullOrEmpty(searchString)) {
                classunitckpts = classunitckpts.Where(s => s.CkptId.Contains(searchString)
                                       || s.Content.Contains(searchString));
            }
            //switch (sortOrder) {
            //    case "signup_desc":
            //        classes = classes.OrderByDescending(x => x.SignupEnabled);
            //        break;
            //    case "checkin":
            //        classes = classes.OrderBy(x => x.CheckinEnabled);
            //        break;
            //    case "checkin_desc":
            //        classes = classes.OrderByDescending(x => x.CheckinEnabled);
            //        break;
            //    default:
            //        classes = classes.OrderBy(x => x.SignupEnabled);
            //        break;
            //}
            int pageSize = 3;
            var fY111Context = _context.ClassUnitCkpts.Include(c => c.ClassUnit);
            return View(await PaginatedList<ClassUnitCkpt>.CreateAsync(classunitckpts.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: ClassUnitCkptsManage/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var classunitckpts = await _context.ClassUnitCkpts
                .FirstOrDefaultAsync(m => m.CkptId == id);
            if (classunitckpts == null) {
                return NotFound();
            }

            return View(classunitckpts);
        }

        // GET: ClassUnitCkptsManage/Create
        public IActionResult Create()
        {
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id");
            return View();
        }

        // POST: ClassUnitCkptsManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassUnitId,CkptId,Content")] ClassUnitCkpt classunitckpts)
        {
            if (ModelState.IsValid) {
                _context.Add(classunitckpts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classunitckpts.ClassUnitId);
            return View(classunitckpts);
        }

        // GET: ClassUnitCkptsManage/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var classunitckpts = await _context.ClassUnitCkpts.FirstOrDefaultAsync(m => m.CkptId == id);
            //var classunitid = await _context.ClassUnitCkpts.FirstOrDefaultAsync(m => m.ClassUnitId == sid);
            if (classunitckpts == null) {
                return NotFound();
            }
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classunitckpts.ClassUnitId);
            return View(classunitckpts);
        }

        // POST: ClassUnitCkptsManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ClassUnitId,CkptId,Content")] ClassUnitCkpt classunitckpts)
        {
            if (id != classunitckpts.CkptId) {
                var originckpt = await _context.ClassUnitCkpts.FirstOrDefaultAsync(m => m.CkptId == id);
                _context.ClassUnitCkpts.Remove(originckpt);
                _context.Add(classunitckpts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid) 
            {
                try 
                {
                    _context.Update(classunitckpts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ClassUnitCkptExists(classunitckpts.CkptId)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classUnitCkpt.ClassUnitId);
            return View(classunitckpts);
        }

        // GET: ClassUnitCkptsManage/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var classunitckpts = await _context.ClassUnitCkpts
                .FirstOrDefaultAsync(m => m.CkptId == id);
            if (classunitckpts == null) {
                return NotFound();
            }

            return View(classunitckpts);
        }

        // POST: ClassUnitCkptsManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @classunitckpts = await _context.ClassUnitCkpts.FirstOrDefaultAsync(m => m.CkptId == id);
            _context.ClassUnitCkpts.Remove(@classunitckpts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassUnitCkptExists(string id)
        {
            return _context.ClassUnitCkpts.Any(e => e.CkptId == id);
        }
    }
}
