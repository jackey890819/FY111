using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models
{
    public partial class Friend
    {
        public int MemberId { get; set; }
        public int MemberId1 { get; set; }

        public virtual Member Member { get; set; }
        public virtual Member MemberId1Navigation { get; set; }
    }
}
