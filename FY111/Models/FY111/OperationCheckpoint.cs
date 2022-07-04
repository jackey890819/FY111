using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class OperationCheckpoint
    {
        public int OperationLittleunitLogId { get; set; }
        public string CkptId { get; set; }
        public int? PointType { get; set; }

        public virtual OperationLittleunitLog OperationLittleunitLog { get; set; }
    }
}
