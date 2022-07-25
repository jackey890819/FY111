using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Timer
    {
        public string MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
