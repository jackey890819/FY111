using System.ComponentModel.DataAnnotations;
using FY111.Resources;

namespace FY111.Models.FY111
{
    public class SignUpManageModel
    {
        public int Id { get; set; }
        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string Code { get; set; }

        [Display(Name = "ClassName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }

        [Display(Name = "Image", ResourceType = typeof(DisplayAttributeResources))]
        public string Image { get; set; }

        [Display(Name = "Content", ResourceType = typeof(DisplayAttributeResources))]
        public string Content { get; set; }

        [Display(Name = "isSignedUp", ResourceType = typeof(DisplayAttributeResources))]
        public bool isSignedUp { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(DisplayAttributeResources))]
        public int? Duration { get; set; }
    }
}
