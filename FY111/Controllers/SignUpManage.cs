using FY111.Areas.Identity.Data;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers
{
    [Authorize]
    public class SignUpManage : Controller
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;

        public SignUpManage(FY111Context context, UserManager<FY111User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "GroupUser")]
        public async Task<IActionResult> OrganizationSignUp(string sortOrder)
        {
            string user_id = _userManager.GetUserId(User);
            var trainings = await _context.training.Where(t => DateTime.Compare((DateTime)t.StartDate, DateTime.Now) < 0 && DateTime.Compare((DateTime)t.EndDate, DateTime.Now) > 0)
                .Include(t => t.ClassSignups.Where(c => c.MemberId == user_id)).Include(t => t.Class).ToListAsync();
            FY111User user = await _userManager.GetUserAsync(User);
            ViewData["Organization"] = user.Organization;
            return View(trainings);
        }

        [Authorize(Roles = "GroupUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OrganizationSignUp(int id, DateTime date)
        {
            List<ClassSignup> add = new List<ClassSignup>();
            FY111User organization_admin = await _userManager.GetUserAsync(User);
            List<string> organization_id = await _userManager.Users.Where(x => x.Organization == organization_admin.Organization).Select(x => x.Id).ToListAsync();
            training t = await _context.training.FindAsync(id);
            foreach (string memberid in organization_id) {
                if (_context.ClassSignups.Any(x => x.MemberId == memberid && x.TrainingId == t.Id && x.Date == date)) {
                    return RedirectToAction(nameof(PersonalSignUp));
                }
                var result = _context.ClassSignups.FirstOrDefault(x => x.TrainingId == t.Id && x.MemberId == memberid);
                if (result == null) {
                    ClassSignup classSignup = new ClassSignup();
                    classSignup.TrainingId = t.Id;
                    classSignup.MemberId = memberid;
                    classSignup.Date = date;
                    add.Add(classSignup);
                }
            }
            _context.ClassSignups.AddRange(add);
            _context.SaveChanges();

            return RedirectToAction(nameof(OrganizationSignUp));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOrganizationSignUp(int id, DateTime date)
        {
            List<ClassSignup> remove = new List<ClassSignup>();
            FY111User organization_admin = await _userManager.GetUserAsync(User);
            List<string> organization_id = await _userManager.Users.Where(x => x.Organization == organization_admin.Organization).Select(x => x.Id).ToListAsync();
            training t = await _context.training.FindAsync(id);
            foreach (string memberid in organization_id) {
                var result = _context.ClassSignups.FirstOrDefault(x => x.TrainingId == id && x.MemberId == memberid && x.Date == date);
                if (result != null) remove.Add(result);
            }
            _context.ClassSignups.RemoveRange(remove);
            _context.SaveChanges();
            return RedirectToAction(nameof(OrganizationSignUp));
        }

        public async Task<IActionResult> PersonalSignUp(string sortOrder)
        {
            string user_id = _userManager.GetUserId(User);
            var trainings = await _context.training.Where(t => DateTime.Compare((DateTime)t.StartDate, DateTime.Now) < 0 && DateTime.Compare((DateTime)t.EndDate, DateTime.Now) > 0)
                .Include(t => t.ClassSignups.Where(c => c.MemberId == user_id)).Include(t => t.Class).ToListAsync();
            return View(trainings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PersonalSignUp(int id, DateTime date)
        {
            if (ModelState.IsValid)
            {
                string user_id = _userManager.GetUserId(User);
                if (_context.ClassSignups.Any(x => x.MemberId == user_id && x.TrainingId == id && x.Date == date))
                {
                    return RedirectToAction(nameof(PersonalSignUp));
                }
                ClassSignup signup = new ClassSignup();
                signup.TrainingId = id;
                signup.MemberId = user_id;
                signup.Date = date;
                _context.Add(signup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PersonalSignUp));
            }
            return RedirectToAction(nameof(PersonalSignUp));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePersonalSignUp(int id, DateTime date)
        {
            string user_id = _userManager.GetUserId(User);
            ClassSignup s = await _context.ClassSignups.FirstOrDefaultAsync(x => x.MemberId == user_id && x.TrainingId == id && x.Date == date);
            _context.Remove(s);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PersonalSignUp));
        }
    }
}
