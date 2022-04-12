using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models
{
    public partial class MemberHasDevice
    {
        public int DeviceType { get; set; }
        public int MemberId { get; set; }

        public virtual Device DeviceTypeNavigation { get; set; }
        public virtual Member Member { get; set; }
    }
}
