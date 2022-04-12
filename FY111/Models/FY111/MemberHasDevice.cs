using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class MemberHasDevice
    {
        public MemberHasDevice()
        {
            Logs = new HashSet<Log>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public int DeviceId { get; set; }
        public string MacAddress { get; set; }

        public virtual Device Device { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
