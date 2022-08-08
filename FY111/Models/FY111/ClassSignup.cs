using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassSignup
    {
        public string MemberId { get; set; }
        public int TrainingId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(DisplayAttributeResources))]
        public DateTime Date { get; set; }
        public virtual training Training { get; set; }
    }
}
