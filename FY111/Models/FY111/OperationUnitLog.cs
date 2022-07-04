using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class OperationUnitLog
    {
        public OperationUnitLog()
        {
            OperationLittleunitLogs = new HashSet<OperationLittleunitLog>();
        }

        public int Id { get; set; }
        public string MemberId { get; set; }
        public string UnitCode { get; set; }
        public byte? Pass { get; set; }

        public virtual ICollection<OperationLittleunitLog> OperationLittleunitLogs { get; set; }
    }
}
