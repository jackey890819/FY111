using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FY111.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<FY111User> _userManager;
        private readonly SignInManager<FY111User> _signInManager;
        public string UserId;

        public IndexModel(
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Avater")]
            public string Avater { get; set; }
        }

        private async Task LoadAsync(FY111User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var avater = user.Avatar;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Avater = avater
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserId = _userManager.GetUserId(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserId}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if(Input.Avater != null)
            {
                var prevAvater = user.Avatar;
                var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\User\\" + user.Id + "\\");
                System.IO.File.Delete(dirPath + prevAvater);
                user.Avatar = Input.Avater;
                var setAvaterResult = await _userManager.UpdateAsync(user);
                if (!setAvaterResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set avater.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
