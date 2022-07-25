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
    [Authorize(Roles = "GroupUser")]
    public class SignUpManage : Controller
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;

        public SignUpManage(FY111Context context, UserManager<FY111User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OrganizationSignUp(string sortOrder)
        {
            ViewData["SignUpParm"] = String.IsNullOrEmpty(sortOrder) ? "signup_desc" : "";
            var classes = await _context.Classes.Where(x => x.SignupEnabled == 1).ToListAsync();
            FY111User user = await _userManager.GetUserAsync(User);
            List<SignUpManageModel> manageModel = new List<SignUpManageModel>();
            foreach (var c in classes)
            {
                SignUpManageModel model = new SignUpManageModel();
                model.Id = c.Id;
                model.Name = c.Name;
                model.Image = c.Image;
                model.Content = c.Content;
                model.Duration = c.Duration;
                bool result = _context.ClassSignups.Where(x => x.ClassId == c.Id).Select(x => x.MemberId).Contains(user.Id);
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
                        var result = _context.ClassSignups.FirstOrDefault(x => x.ClassId == model.Id && x.MemberId == id);
                        if (result == null)
                        {
                            ClassSignup classSignup = new ClassSignup();
                            classSignup.ClassId = model.Id;
                            classSignup.MemberId = id;
                            add.Add(classSignup);
                        }
                    }
                }
                else
                {
                    foreach (string id in organization_id)
                    {
                        var result = _context.ClassSignups.FirstOrDefault(x => x.ClassId == model.Id && x.MemberId == id);
                        if (result != null) remove.Add(result);
                    }
                }
            }
            _context.ClassSignups.AddRange(add);
            _context.ClassSignups.RemoveRange(remove);
            _context.SaveChanges();
            return RedirectToAction(nameof(OrganizationSignUp));
        }
    }
}
