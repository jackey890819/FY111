using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class OperationOccdisaster
    {
        public int OperationLittleunitLogId { get; set; }
        public string OccDisasterCode { get; set; }

        public virtual OperationLittleunitLog OperationLittleunitLog { get; set; }
    }
}
