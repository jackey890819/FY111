using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FY111.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        //public void OnGet()
        //{

        //}
        public IActionResult OnGet()
        {
            return BadRequest(new { message = "AccessDenied. Permission error." });
        }
    }
}

