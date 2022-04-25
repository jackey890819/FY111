using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Metaverse
    {
        public Metaverse()
        {
            MetaverseLogs = new HashSet<MetaverseLog>();
            MetaverseSignIns = new HashSet<MetaverseSignIn>();
            MetaverseSignUps = new HashSet<MetaverseSignUp>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Icon { get; set; }
        public string Introduction { get; set; }
        public byte SigninEnabled { get; set; }
        public byte SignupEnabled { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<MetaverseLog> MetaverseLogs { get; set; }
        public virtual ICollection<MetaverseSignIn> MetaverseSignIns { get; set; }
        public virtual ICollection<MetaverseSignUp> MetaverseSignUps { get; set; }
    }
}
