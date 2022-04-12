using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models
{
    public partial class MemberHasGroup
    {
        public int MemberId { get; set; }
        public string GroupName { get; set; }

        public virtual Group GroupNameNavigation { get; set; }
        public virtual Member Member { get; set; }
    }
}
