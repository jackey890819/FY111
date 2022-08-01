using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnitApp
    {
        public int Id { get; set; }
        public int ClassUnitId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "Application", ResourceType = typeof(DisplayAttributeResources))]
        public string Application { get; set; }
        [Display(Name = "Content", ResourceType = typeof(DisplayAttributeResources))]
        public string Content { get; set; }

        public virtual ClassUnit ClassUnit { get; set; }
    }
}
