using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class training
    {
        public training()
        {
            ClassCheckins = new HashSet<ClassCheckin>();
            ClassSignups = new HashSet<ClassSignup>();
        }

        public int Id { get; set; }
        [Display(Name = "ClassName", ResourceType = typeof(DisplayAttributeResources))]
        public int ClassId { get; set; }
        [Display(Name = "SessionName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "SignupEnabled", ResourceType = typeof(DisplayAttributeResources))]
        public byte SignupEnabled { get; set; }
        [Display(Name = "CheckinEnabled", ResourceType = typeof(DisplayAttributeResources))]
        public byte CheckinEnabled { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(DisplayAttributeResources))]
        public DateTime? StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(DisplayAttributeResources))]
        public DateTime? EndDate { get; set; }
        [Display(Name = "StartTime", ResourceType = typeof(DisplayAttributeResources))]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "EndTime", ResourceType = typeof(DisplayAttributeResources))]
        public TimeSpan? EndTime { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ClassCheckin> ClassCheckins { get; set; }
        public virtual ICollection<ClassSignup> ClassSignups { get; set; }
    }
}
