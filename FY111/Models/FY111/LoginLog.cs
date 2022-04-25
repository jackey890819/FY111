using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class LoginLog
    {
        public int MemberId { get; set; }
        public int DeviceType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Device DeviceTypeNavigation { get; set; }
        public virtual Member Member { get; set; }
    }
}
