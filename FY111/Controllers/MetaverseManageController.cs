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

            var mateverse = _context.Metaverses.Select(x=>x);
            if (!String.IsNullOrEmpty(searchString))
            {
                mateverse = mateverse.Where(s => s.Name.Contains(searchString)
                                       || s.Introduction.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "signup_desc":
                    mateverse = mateverse.OrderByDescending(x => x.SignupEnabled);
                    break;
                case "checkin":
                    mateverse = mateverse.OrderBy(x => x.CheckinEnabled);
                    break;
                case "checkin_desc":
                    mateverse = mateverse.OrderByDescending(x => x.CheckinEnabled);
                    break;
                default:
                    mateverse = mateverse.OrderBy(x => x.SignupEnabled);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Metaverse>.CreateAsync(mateverse.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await mateverse.AsNoTracking().ToListAsync());
        }

        // GET: MetaverseManage
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Metaverses.ToListAsync());
        //}

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
                    if (metaverse.Icon == null)
                    {
                        string result = (await _context.Metaverses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Icon;
                        metaverse.Icon = result;
                    }
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
