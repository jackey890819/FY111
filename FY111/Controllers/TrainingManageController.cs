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
    public class TrainingManageController : Controller
    {
        private readonly FY111Context _context;

        public TrainingManageController(FY111Context context)
        {
            _context = context;
        }

        // GET: TrainingManage
        public async Task<IActionResult> Index()
        {
            var fY111Context = _context.training.Include(t => t.Class);
            //var fY111Context = _context.training.Where(t => DateTime.Compare((DateTime)t.Date, DateTime.Now) > 0).Include(t => t.Class);
            return View(await fY111Context.ToListAsync());
        }

        // GET: TrainingManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .Include(t => t.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: TrainingManage/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            return View();
        }

        // POST: TrainingManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassId,Name,Date,StartTime,EndTime")] training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", training.ClassId);
            return View(training);
        }

        // GET: TrainingManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", training.ClassId);
            return View(training);
        }

        // POST: TrainingManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassId,Name,Date,StartTime,EndTime")] training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!trainingExists(training.Id))
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", training.ClassId);
            return View(training);
        }

        // GET: TrainingManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .Include(t => t.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: TrainingManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.training.FindAsync(id);
            _context.training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool trainingExists(int id)
        {
            return _context.training.Any(e => e.Id == id);
        }
    }
}
