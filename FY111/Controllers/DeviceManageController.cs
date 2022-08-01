using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace FY111.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeviceManageController : Controller
    {
        private readonly FY111Context _context;

        public DeviceManageController(FY111Context context)
        {
            _context = context;
        }

        // GET: DeviceManage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Devices.ToListAsync());
        }

        // GET: DeviceManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: DeviceManage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeviceManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Icon,Name")] Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // GET: DeviceManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // POST: DeviceManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Icon,Name")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string result = (await _context.Devices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)).Icon;
                    if (device.Icon == null)
                    {
                        device.Icon = result;
                    }
                    else if (result != null && device.Icon != result)
                    {
                        var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\Device\\");
                        System.IO.File.Delete(dirPath + result);
                    }
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
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
            return View(device);
        }

        // GET: DeviceManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            string result = device.Icon;
            if (device.Icon != null)
            {
                var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\Device\\");
                System.IO.File.Delete(dirPath + result);
            }
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: DeviceManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
