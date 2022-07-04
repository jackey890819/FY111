using System.ComponentModel.DataAnnotations;
using FY111.Resources;

namespace FY111.Models.FY111User
{
    public class ManageModel
    {
        public string Id { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(DisplayAttributeResources))]
        public string UserName { get; set; }

        [Display(Name = "Avatar", ResourceType = typeof(DisplayAttributeResources))]
        public string Avatar { get; set; }

        [Display(Name = "Email", ResourceType = typeof(DisplayAttributeResources))]
        public string Email { get; set; }

        [Display(Name = "Organization", ResourceType = typeof(DisplayAttributeResources))]
        public string Organization { get; set; }

        [Display(Name = "Role", ResourceType = typeof(DisplayAttributeResources))]
        public string Role { get; set; }
    }
}
