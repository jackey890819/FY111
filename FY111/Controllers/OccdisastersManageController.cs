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
    public class OccdisastersManageController : Controller
    {
        private readonly FY111Context _context;

        public OccdisastersManageController(FY111Context context)
        {
            _context = context;
        }

        // GET: OccdisastersManage
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
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

            var occdisasters = _context.Occdisasters.Select(x => x);
            if (!String.IsNullOrEmpty(searchString)) 
            {
                occdisasters = occdisasters.Where(s => s.Content.Contains(searchString));
            }
            //switch (sortOrder) {
            //    case "signup_desc":
            //        occdisasters = occdisasters.OrderByDescending(x => x.SignupEnabled);
            //        break;
            //    case "checkin":
            //        occdisasters = occdisasters.OrderBy(x => x.CheckinEnabled);
            //        break;
            //    case "checkin_desc":
            //        occdisasters = occdisasters.OrderByDescending(x => x.CheckinEnabled);
            //        break;
            //    default:
            //        occdisasters = occdisasters.OrderBy(x => x.SignupEnabled);
            //        break;
            //}
            int pageSize = 3;
            return View(await PaginatedList<Occdisaster>.CreateAsync(occdisasters.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: OccdisastersManage/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occdisaster = await _context.Occdisasters
                .FirstOrDefaultAsync(m => m.Code == id);
            if (occdisaster == null)
            {
                return NotFound();
            }

            return View(occdisaster);
        }

        // GET: OccdisastersManage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OccdisastersManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Content")] Occdisaster occdisaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(occdisaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(occdisaster);
        }

        // GET: OccdisastersManage/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occdisaster = await _context.Occdisasters.FindAsync(id);
            if (occdisaster == null) {
                return NotFound();
            }
            return View(occdisaster);
        }

        // POST: OccdisastersManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Code,Content")] Occdisaster occdisaster)
        {
            if (id != occdisaster.Code) {
                var originOccdisaster = await _context.Occdisasters.FindAsync(id);
                _context.Occdisasters.Remove(originOccdisaster);
                _context.Add(occdisaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(occdisaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OccdisasterExists(occdisaster.Code))
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
            return View(occdisaster);
        }

        // GET: OccdisastersManage/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occdisaster = await _context.Occdisasters
                .FirstOrDefaultAsync(m => m.Code == id);
            if (occdisaster == null)
            {
                return NotFound();
            }

            return View(occdisaster);
        }

        // POST: OccdisastersManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @occdisaster = await _context.Occdisasters.FindAsync(id);
            _context.Occdisasters.Remove(@occdisaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OccdisasterExists(string id)
        {
            return _context.Occdisasters.Any(e => e.Code == id);
        }
    }
}
