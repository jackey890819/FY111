using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FY111.Resources;
using Microsoft.Extensions.Localization;

namespace FY111.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<FY111User> _userManager;
        private readonly SignInManager<FY111User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IStringLocalizer<LoginModel> _localizer;

        public LoginModel(SignInManager<FY111User> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<FY111User> userManager,
            IStringLocalizer<LoginModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            /*
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            */
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [StringLength(256, ErrorMessageResourceName = "CharacterLimitation", MinimumLength = 6, ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [DataType(DataType.Text)]
            [Display(Name = "UserName", ResourceType = typeof(DisplayAttributeResources))]
            public string UserName { get; set; }
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
            [DataType(DataType.Password)]
            [Display(Name = "Password", ResourceType = typeof(DisplayAttributeResources))]
            public string Password { get; set; }

            [Display(Name = "Remember", ResourceType = typeof(DisplayAttributeResources))]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["Invalid login attempt."]);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
