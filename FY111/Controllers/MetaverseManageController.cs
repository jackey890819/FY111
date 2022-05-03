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
    public class MetaverseManageController : Controller
    {
        private readonly FY111Context _context;

        public MetaverseManageController(FY111Context context)
        {
            _context = context;
        }

        // GET: MetaverseManage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Metaverses.ToListAsync());
        }

        // GET: MetaverseManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaverse = await _context.Metaverses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaverse == null)
            {
                return NotFound();
            }

            return View(metaverse);
        }

        // GET: MetaverseManage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MetaverseManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Ip,Icon,Introduction,SignupEnabled,CheckinEnabled,Duration")] Metaverse metaverse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metaverse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metaverse);
        }

        // GET: MetaverseManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaverse = await _context.Metaverses.FindAsync(id);
            if (metaverse == null)
            {
                return NotFound();
            }
            return View(metaverse);
        }

        // POST: MetaverseManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Ip,Icon,Introduction,SignupEnabled,CheckinEnabled,Duration")] Metaverse metaverse)
        {
            if (id != metaverse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaverse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaverseExists(metaverse.Id))
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
            return View(metaverse);
        }

        // GET: MetaverseManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaverse = await _context.Metaverses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaverse == null)
            {
                return NotFound();
            }

            return View(metaverse);
        }

        // POST: MetaverseManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metaverse = await _context.Metaverses.FindAsync(id);
            _context.Metaverses.Remove(metaverse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetaverseExists(int id)
        {
            return _context.Metaverses.Any(e => e.Id == id);
        }
    }
}
