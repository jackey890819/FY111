using System.ComponentModel.DataAnnotations;
using FY111.Resources;

namespace FY111.Models.FY111User
{
    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
        [StringLength(256, ErrorMessageResourceName = "CharacterLimitation", MinimumLength = 6, ErrorMessageResourceType = typeof(ErrorMessageResources))]
        [DataType(DataType.Text)]
        [Display(Name = "UserName", ResourceType = typeof(DisplayAttributeResources))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ErrorMessageResources))]
        [StringLength(100, ErrorMessageResourceName = "CharacterLimitation", MinimumLength = 6, ErrorMessageResourceType = typeof(ErrorMessageResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DisplayAttributeResources))]
        public string Password { get; set; }
    }
}

