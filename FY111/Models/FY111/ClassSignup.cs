using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassSignup
    {
        public string MemberId { get; set; }
        public int TrainingId { get; set; }

        public virtual training Training { get; set; }
    }
}
