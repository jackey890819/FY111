using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FY111.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FY111User class
    public class FY111User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(256)")]
        public string Avatar { get; set; }


        [PersonalData]
        [Column(TypeName = "nvarchar(256)")]
        public string Organization { get; set; }
    }
}
