using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models
{
    public partial class Device
    {
        public Device()
        {
            MemberHasDevices = new HashSet<MemberHasDevice>();
        }

        public int Type { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MemberHasDevice> MemberHasDevices { get; set; }
    }
}
