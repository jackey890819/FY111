using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnitApp
    {
        public int Id { get; set; }
        public int ClassUnitId { get; set; }
        public string Name { get; set; }
        public string Application { get; set; }
        public string Content { get; set; }

        public virtual ClassUnit ClassUnit { get; set; }
    }
}
