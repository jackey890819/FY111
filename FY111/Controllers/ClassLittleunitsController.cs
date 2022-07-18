using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;

namespace FY111.Controllers
{
    public class ClassLittleunitsController : Controller
    {
        private readonly FY111Context _context;

        public ClassLittleunitsController(FY111Context context)
        {
            _context = context;
        }

        // GET: ClassLittleunits
        public async Task<IActionResult> Index()
        {
            var fY111Context = _context.ClassLittleunits.Include(c => c.ClassUnit);
            return View(await fY111Context.ToListAsync());
        }

        // GET: ClassLittleunits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classLittleunit = await _context.ClassLittleunits
                .Include(c => c.ClassUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classLittleunit == null)
            {
                return NotFound();
            }

            return View(classLittleunit);
        }

        // GET: ClassLittleunits/Create
        public IActionResult Create()
        {
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id");
            return View();
        }

        // POST: ClassLittleunits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassUnitId,Code,Name,Image")] ClassLittleunit classLittleunit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classLittleunit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classLittleunit.ClassUnitId);
            return View(classLittleunit);
        }

        // GET: ClassLittleunits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classLittleunit = await _context.ClassLittleunits.FindAsync(id);
            if (classLittleunit == null)
            {
                return NotFound();
            }
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classLittleunit.ClassUnitId);
            return View(classLittleunit);
        }

        // POST: ClassLittleunits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassUnitId,Code,Name,Image")] ClassLittleunit classLittleunit)
        {
            if (id != classLittleunit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classLittleunit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassLittleunitExists(classLittleunit.Id))
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
            ViewData["ClassUnitId"] = new SelectList(_context.ClassUnits, "Id", "Id", classLittleunit.ClassUnitId);
            return View(classLittleunit);
        }

        // GET: ClassLittleunits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classLittleunit = await _context.ClassLittleunits
                .Include(c => c.ClassUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classLittleunit == null)
            {
                return NotFound();
            }

            return View(classLittleunit);
        }

        // POST: ClassLittleunits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classLittleunit = await _context.ClassLittleunits.FindAsync(id);
            _context.ClassLittleunits.Remove(classLittleunit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassLittleunitExists(int id)
        {
            return _context.ClassLittleunits.Any(e => e.Id == id);
        }
    }
}
