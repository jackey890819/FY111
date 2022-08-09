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
            TrainingCheckins = new HashSet<TrainingCheckin>();
            TrainingSignups = new HashSet<TrainingSignup>();
        }

        public int Id { get; set; }
        [Display(Name = "SessionName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(DisplayAttributeResources))]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(DisplayAttributeResources))]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "StartTime", ResourceType = typeof(DisplayAttributeResources))]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "EndTime", ResourceType = typeof(DisplayAttributeResources))]
        public TimeSpan? EndTime { get; set; }

        public virtual ICollection<TrainingCheckin> TrainingCheckins { get; set; }
        public virtual ICollection<TrainingSignup> TrainingSignups { get; set; }
    }
}
