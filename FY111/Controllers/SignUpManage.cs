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
            ViewData["SignUpParm"] = String.IsNullOrEmpty(sortOrder) ? "signup_desc" : "";
            //var classes = await _context.Classes.Where(x => x.SignupEnabled == 1).ToListAsync();
            var timecompare = await _context.training.Where(t => DateTime.Compare((DateTime)t.StartDate, DateTime.Now) > 0).Include(t => t.Class).ToListAsync();
            FY111User user = await _userManager.GetUserAsync(User);
            List<SignUpManageModel> manageModel = new List<SignUpManageModel>();
            foreach (var c in timecompare)
            {
                SignUpManageModel model = new SignUpManageModel();
                model.Id = c.Id;
                model.Name = c.Name;
                model.ClassId = c.Class.Name;
                model.date = c.StartDate;
                model.StartTime = c.StartTime;
                model.EndTime = c.EndTime;
                bool result = _context.ClassSignups.Where(x => x.TrainingId == c.Id).Select(x => x.MemberId).Contains(user.Id);
                model.isSignedUp = result;
                manageModel.Add(model);
            }
            switch (sortOrder)
            {
                case "signup_desc":
                    manageModel = manageModel.OrderByDescending(x => x.isSignedUp).ToList();
                    break;
                default:
                    manageModel = manageModel.OrderBy(x => x.isSignedUp).ToList();
                    break;
            }

            ViewData["Organization"] = user.Organization;
            return View(manageModel);
        }

        [Authorize(Roles = "GroupUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OrganizationSignUp(List<OrganizationSignUpModel> models)
        {
            List<ClassSignup> add = new List<ClassSignup>();
            List<ClassSignup> remove = new List<ClassSignup>();
            FY111User organization_admin = await _userManager.GetUserAsync(User);
            List<string> organization_id = await _userManager.Users.Where(x => x.Organization == organization_admin.Organization).Select(x => x.Id).ToListAsync();
            foreach (OrganizationSignUpModel model in models)
            {
                if (model.IsSignedUp)
                {
                    foreach (string id in organization_id)
                    {
                        var result = _context.ClassSignups.FirstOrDefault(x => x.TrainingId == model.Id && x.MemberId == id);
                        if (result == null)
                        {
                            ClassSignup classSignup = new ClassSignup();
                            classSignup.TrainingId = model.Id;
                            classSignup.MemberId = id;
                            add.Add(classSignup);
                        }
                    }
                }
                else
                {
                    foreach (string id in organization_id)
                    {
                        var result = _context.ClassSignups.FirstOrDefault(x => x.TrainingId == model.Id && x.MemberId == id);
                        if(result != null) remove.Add(result);
                    }
                }
            }
            _context.ClassSignups.AddRange(add);
            _context.ClassSignups.RemoveRange(remove);
            _context.SaveChanges();
            return RedirectToAction(nameof(OrganizationSignUp));
        }

        public async Task<IActionResult> PersonalSignUp(string sortOrder)
        {
            FY111User user = await _userManager.GetUserAsync(User);
            var trainings = await _context.training.Where(t => DateTime.Compare((DateTime)t.StartDate, DateTime.Now) < 0 && DateTime.Compare((DateTime)t.EndDate, DateTime.Now) > 0)
                .Include(t => t.ClassSignups).Include(t => t.Class).ToListAsync();
            //foreach (var training in trainings)
            //{
            //    training.ClassSignups = await _context.ClassSignups.Where(x => x.TrainingId == training.Id && x.MemberId == user.Id).ToListAsync();
            //}
            return View(trainings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PersonalSignUp(int id, DateTime date)
        {
            training t = await _context.training.FindAsync(id);
            ClassSignup signup = new ClassSignup();
            signup.TrainingId = id;
            signup.MemberId = _userManager.GetUserId(User);
            signup.Date = date;
            _context.Add(signup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PersonalSignUp));
        }
    }
}
