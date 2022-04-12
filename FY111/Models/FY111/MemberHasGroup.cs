using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class MemberHasGroup
    {
        public int MemberId { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Member Member { get; set; }
    }
}
