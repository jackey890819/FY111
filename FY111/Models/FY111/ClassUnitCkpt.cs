using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnitCkpt
    {
        public int ClassUnitId { get; set; }
        public string CkptId { get; set; }
        public string Content { get; set; }

        public virtual ClassUnit ClassUnit { get; set; }
    }
}
