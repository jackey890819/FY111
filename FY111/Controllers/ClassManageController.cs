using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.IO;

namespace FY111.Controllers
{
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

            var classes = _context.Classes.Select(x=>x);
            if (!String.IsNullOrEmpty(searchString))
            {
                classes = classes.Where(s => s.Name.Contains(searchString)
                                       || s.Content.Contains(searchString));
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
        public async Task<IActionResult> Create([Bind("Id,Name,Ip,Image,Content,SignupEnabled,CheckinEnabled,Duration")] Class classes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classes);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Ip,Image,Content,SignupEnabled,CheckinEnabled,Duration")] Class @class)
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
                        System.IO.File.Delete(dirPath+imageName);
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

        // POST: ClassManage/Delete/5
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

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
