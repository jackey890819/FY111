using System;
using System.ComponentModel.DataAnnotations;
using FY111.Resources;

namespace FY111.Models.FY111
{
    public class SignUpManageModel
    {
        public int Id { get; set; }
        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string Code { get; set; }
        [Display(Name = "SessionName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        [Display(Name = "isSignedUp", ResourceType = typeof(DisplayAttributeResources))]
        public bool isSignedUp { get; set; }
        [Display(Name = "ClassName", ResourceType = typeof(DisplayAttributeResources))]
        public string ClassId { get; set; }
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
        //[Display(Name = "Date", ResourceType = typeof(DisplayAttributeResources))]
        [DataType(DataType.Date)]
        public DateTime? date { get; set; }
        public virtual Class Class { get; set; }
    }
}
