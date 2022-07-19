using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FY111.Models.FY111User;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserManageController : Controller
    {
        private UserManager<FY111User> _userManager;
        

        public UserManageController(UserManager<FY111User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["RoleParm"] = String.IsNullOrEmpty(sortOrder) ? "role_desc" : "";
            ViewData["OrganizationParm"] = sortOrder == "organization" ? "organization_desc" : "organization";
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
            List<FY111User> Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                Users = await _userManager.Users.Where(x => x.UserName.Contains(searchString)).ToListAsync();
            }
            else Users = await _userManager.Users.ToListAsync();

            List<ManageModel> manageModel = new List<ManageModel>();
            for (int i = 0; i < Users.Count; i++)
            {
                ManageModel model = new ManageModel();
                model.UserName = Users[i].UserName;
                model.Id = Users[i].Id;
                model.Email = Users[i].Email;
                model.Avatar = Users[i].Avatar;
                model.Organization = Users[i].Organization;
                model.Role = (await _userManager.GetRolesAsync(Users[i]))[0];
                manageModel.Add(model);
            }
            switch (sortOrder)
            {
                case "role_desc":
                    manageModel = manageModel.OrderByDescending(x => x.Role).ToList();
                    break;
                case "organization":
                    manageModel = manageModel.OrderBy(x => x.Organization).ToList();
                    break;
                case "organization_desc":
                    manageModel = manageModel.OrderByDescending(x => x.Organization).ToList();
                    break;
                default:
                    manageModel = manageModel.OrderBy(x => x.Role).ToList();
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<ManageModel>.CreateAsync(manageModel, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ManageModel model = new ManageModel();
            model.UserName = user.UserName;
            model.Id = user.Id;
            model.Email = user.Email;
            model.Avatar = user.Avatar;
            model.Organization = user.Organization;
            model.Role = (await _userManager.GetRolesAsync(user))[0];
            return View(model);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ManageModel model = new ManageModel();
            model.UserName = user.UserName;
            model.Id = user.Id;
            model.Email = user.Email;
            model.Avatar = user.Avatar;
            model.Organization = user.Organization;
            model.Role = (await _userManager.GetRolesAsync(user))[0];
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ManageModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                FY111User user = await _userManager.FindByIdAsync(model.Id);
                string prevRole = (await _userManager.GetRolesAsync(user))[0];
                if(prevRole != model.Role)
                {
                    await _userManager.RemoveFromRoleAsync(user, prevRole);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                user.Organization = model.Organization;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ManageModel model = new ManageModel();
            model.UserName = user.UserName;
            model.Id = user.Id;
            model.Email = user.Email;
            model.Avatar = user.Avatar;
            model.Organization = user.Organization;
            model.Role = (await _userManager.GetRolesAsync(user))[0];
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Organization()
        {
            var user = await _userManager.GetUserAsync(User);
            var Users = await _userManager.Users.Where(x => x.Organization == user.Organization).ToListAsync();
            List<ManageModel> manageModel = new List<ManageModel>(); 
            for (int i = 0; i < Users.Count; i++)
            {
                var role = (await _userManager.GetRolesAsync(Users[i]))[0];
                if (role == "NormalUser")
                {
                    ManageModel model = new ManageModel();
                    model.UserName = Users[i].UserName;
                    model.Id = Users[i].Id;
                    model.Email = Users[i].Email;
                    model.Avatar = Users[i].Avatar;
                    manageModel.Add(model);
                }
            }

            ViewData["Organization"] = user.Organization;
            return View(manageModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var exist = await _userManager.FindByNameAsync(model.UserName);
                if (exist == null)
                {
                    var organization = (await _userManager.GetUserAsync(User)).Organization;
                    var user = new FY111User { UserName = model.UserName };
                    user.Organization = organization;
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "NormalUser");
                        return RedirectToAction(nameof(Organization));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName exist");
                    return View(model);
                }
                    
            }
            return View(model);
        }
    }
}