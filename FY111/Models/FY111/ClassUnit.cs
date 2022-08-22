using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnit
    {
        public ClassUnit()
        {
            ClassLittleunits = new HashSet<ClassLittleunit>();
            ClassUnitApps = new HashSet<ClassUnitApp>();
        }

        public int Id { get; set; }
        public int ClassId { get; set; }
        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string Code { get; set; }
        [Display(Name = "ClassUnitName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "Image", ResourceType = typeof(DisplayAttributeResources))]
        public string Image { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ClassLittleunit> ClassLittleunits { get; set; }
        public virtual ICollection<ClassUnitApp> ClassUnitApps { get; set; }
    }
}
