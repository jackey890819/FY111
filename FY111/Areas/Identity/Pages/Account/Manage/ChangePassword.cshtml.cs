using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FY111.Resources;
using Microsoft.Extensions.Localization;

namespace FY111.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<FY111User> _userManager;
        private readonly SignInManager<FY111User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IStringLocalizer<ChangePasswordModel> _localizer;

        public ChangePasswordModel(
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager,
            ILogger<ChangePasswordModel> logger,
            IStringLocalizer<ChangePasswordModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [DataType(DataType.Password)]
            [Display(Name = "CurrentPassword", ResourceType = typeof(DisplayAttributeResources))]
            public string OldPassword { get; set; }

            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [StringLength(100, ErrorMessageResourceName = "CharacterLimitation", MinimumLength = 6, ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [DataType(DataType.Password)]
            [Display(Name = "NewPassword", ResourceType = typeof(DisplayAttributeResources))]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "NewPasswordConfirm", ResourceType = typeof(DisplayAttributeResources))]
            [Compare("NewPassword", ErrorMessageResourceName ="NewPasswordCompare", ErrorMessageResourceType = typeof(ErrorMessageResources))]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = _localizer["Your password has been changed."];

            return RedirectToPage();
        }
    }
}
