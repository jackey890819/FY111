using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassSignup
    {
        public int TrainingSignupId { get; set; }
        public int ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual TrainingSignup TrainingSignup { get; set; }
    }
}
