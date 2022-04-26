using Microsoft.AspNetCore.Mvc;

namespace FY111.Models
{
    public class ApplicationSettings : Controller
    {
        public string JWT_Secret { get; set; }
        
    }
}
