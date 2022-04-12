using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Log
    {
        public int Id { get; set; }
        public int MemberHasDeviceId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual MemberHasDevice MemberHasDevice { get; set; }
    }
}
