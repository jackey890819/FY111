using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Class
    {
        public Class()
        {
            ClassCheckins = new HashSet<ClassCheckin>();
            ClassSignups = new HashSet<ClassSignup>();
            ClassUnits = new HashSet<ClassUnit>();
        }

        public int Id { get; set; }

        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string Code { get; set; }

        [Display(Name = "ClassName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }

        [Display(Name = "IP")]
        public string Ip { get; set; }

        [Display(Name = "Image", ResourceType = typeof(DisplayAttributeResources))]
        public string Image { get; set; }

        [Display(Name = "Content", ResourceType = typeof(DisplayAttributeResources))]
        public string Content { get; set; }

        [Display(Name = "SignupEnabled", ResourceType = typeof(DisplayAttributeResources))]
        public byte SignupEnabled { get; set; }

        [Display(Name = "CheckinEnabled", ResourceType = typeof(DisplayAttributeResources))]
        public byte CheckinEnabled { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(DisplayAttributeResources))]
        public int? Duration { get; set; }

        public virtual ICollection<ClassCheckin> ClassCheckins { get; set; }
        public virtual ICollection<ClassSignup> ClassSignups { get; set; }
        public virtual ICollection<ClassUnit> ClassUnits { get; set; }
    }
}
