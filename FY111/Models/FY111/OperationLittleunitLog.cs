using FY111.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class OperationLittleunitLog
    {
        public OperationLittleunitLog()
        {
            OperationCheckpoints = new HashSet<OperationCheckpoint>();
            OperationOccdisasters = new HashSet<OperationOccdisaster>();
        }

        public int Id { get; set; }
        public int OperationLogId { get; set; }
        [Display(Name = "Code", ResourceType = typeof(DisplayAttributeResources))]
        public string LittleunitCode { get; set; }
        [Display(Name = "Score", ResourceType = typeof(DisplayAttributeResources))]
        public int? Score { get; set; }
        [Display(Name = "Pass", ResourceType = typeof(DisplayAttributeResources))]
        public byte? Pass { get; set; }
        [Display(Name = "StartTime", ResourceType = typeof(DisplayAttributeResources))]
        public DateTime? StartTime { get; set; }
        [Display(Name = "EndTime", ResourceType = typeof(DisplayAttributeResources))]
        public DateTime? EndTime { get; set; }
        public virtual OperationUnitLog OperationLog { get; set; }
        public virtual ICollection<OperationCheckpoint> OperationCheckpoints { get; set; }
        public virtual ICollection<OperationOccdisaster> OperationOccdisasters { get; set; }
    }
}
