using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly FY111Context _context;

        public ChartsController(FY111Context context)
        {
            _context = context;
        }

        [HttpGet("demo")]
        public IActionResult demo()
        {
            List<string> labels = new List<string>{ "Red", "Blue", "Yellow", "Green", "Purple", "Orange" };
            List<int> datas = new List<int> { 12, 19, 3, 5, 2, 3 };

            return Ok(new
            {
                labels = labels,
                datas = datas
            });
        }

        [HttpGet("MetaverseConnectionStatus")]
        public async Task<IActionResult> getMetaverseConnectionStatus()
        {
            var checkinEnabled = await _context.Classes
                //.Where(x => x.CheckinEnabled == 1)
                .ToListAsync();
            List<string> labels = new List<string> { };
            List<int> datas = new List<int> { };

            foreach(var meta in checkinEnabled)
            {
                int count = await _context.ClassLogs
                    .Where(x => x.ClassId == meta.Id && x.EndTime==null)
                    .CountAsync();
                labels.Add(meta.Name);
                datas.Add(count);
            }

            return Ok(new
            {
                labels = labels,
                datas = datas
            });
        }

    }
}
