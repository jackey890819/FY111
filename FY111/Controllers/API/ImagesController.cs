using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private SignInManager<FY111User> _signInManager;
        private readonly IWebHostEnvironment _env;

        public ImagesController(
            SignInManager<FY111User> signInManager, IWebHostEnvironment hostingEnv)
        {
            _signInManager = signInManager;
            _env = hostingEnv;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            if (_signInManager.IsSignedIn(User))
            {
                foreach (IFormFile source in files)
                {
                    var fileName = source.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await source.CopyToAsync(fileSrteam);
                    }
                }
                return Ok(new { success = true });
            }
            else { return BadRequest(new { success = false }); }   
        }

    }
}
