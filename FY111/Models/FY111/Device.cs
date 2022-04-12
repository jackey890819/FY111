using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Device
    {
        public Device()
        {
            MemberHasDevices = new HashSet<MemberHasDevice>();
        }

        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MemberHasDevice> MemberHasDevices { get; set; }
    }
}
