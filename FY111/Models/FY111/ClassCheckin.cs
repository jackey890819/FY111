using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassCheckin
    {
        public string MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime? Time { get; set; }

        public virtual Class Class { get; set; }
    }
}
