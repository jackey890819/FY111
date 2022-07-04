using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FY111.Dtos
{    public class UserForRegistrationDto
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Organization { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
