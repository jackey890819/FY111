using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnitCkpt
    {
        [Display(Name = "ClassUnitId", ResourceType = typeof(DisplayAttributeResources))]
        public int ClassUnitId { get; set; }
        [Display(Name = "CkptId", ResourceType = typeof(DisplayAttributeResources))]
        public string CkptId { get; set; }
        [Display(Name = "Content", ResourceType = typeof(DisplayAttributeResources))]
        public string Content { get; set; }

        public virtual ClassUnit ClassUnit { get; set; }
    }
}
