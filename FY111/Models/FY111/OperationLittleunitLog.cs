using System;
using System.Collections.Generic;

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
        public string LittleunitCode { get; set; }
        public int? Score { get; set; }
        public byte? Pass { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual OperationUnitLog OperationLog { get; set; }
        public virtual ICollection<OperationCheckpoint> OperationCheckpoints { get; set; }
        public virtual ICollection<OperationOccdisaster> OperationOccdisasters { get; set; }
    }
}
