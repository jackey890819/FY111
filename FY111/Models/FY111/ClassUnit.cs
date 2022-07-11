using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassUnit
    {
        public ClassUnit()
        {
            ClassLittleunits = new HashSet<ClassLittleunit>();
            ClassUnitCkpts = new HashSet<ClassUnitCkpt>();
        }

        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ClassLittleunit> ClassLittleunits { get; set; }
        public virtual ICollection<ClassUnitCkpt> ClassUnitCkpts { get; set; }
    }
}
