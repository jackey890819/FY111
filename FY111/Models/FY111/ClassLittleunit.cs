using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassLittleunit
    {
        public ClassLittleunit()
        {
            ClassUnitCkpts = new HashSet<ClassUnitCkpt>();
            Occdisasters = new HashSet<Occdisaster>();
        }

        public int Id { get; set; }
        public int ClassUnitId { get; set; }
        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string Code { get; set; }
        [Display(Name = "ClassLittleUnitName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "Image", ResourceType = typeof(DisplayAttributeResources))]
        public string Image { get; set; }

        public virtual ClassUnit ClassUnit { get; set; }
        public virtual ICollection<ClassUnitCkpt> ClassUnitCkpts { get; set; }
        public virtual ICollection<Occdisaster> Occdisasters { get; set; }
    }
}
