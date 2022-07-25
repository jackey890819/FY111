using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Class
    {
        public Class()
        {
            ClassLogs = new HashSet<ClassLog>();
            ClassUnits = new HashSet<ClassUnit>();
            training = new HashSet<training>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public byte SignupEnabled { get; set; }
        public byte CheckinEnabled { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<ClassLog> ClassLogs { get; set; }
        public virtual ICollection<ClassUnit> ClassUnits { get; set; }
        public virtual ICollection<training> training { get; set; }
    }
}
