using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassLog
    {
        public string MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Class Class { get; set; }
    }
}
