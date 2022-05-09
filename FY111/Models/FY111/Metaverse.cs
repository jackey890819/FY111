using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Metaverse
    {
        public Metaverse()
        {
            MetaverseCheckins = new HashSet<MetaverseCheckin>();
            MetaverseLogs = new HashSet<MetaverseLog>();
            MetaverseSignups = new HashSet<MetaverseSignup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Icon { get; set; }
        public string Introduction { get; set; }
        public byte SignupEnabled { get; set; }
        public byte CheckinEnabled { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<MetaverseCheckin> MetaverseCheckins { get; set; }
        public virtual ICollection<MetaverseLog> MetaverseLogs { get; set; }
        public virtual ICollection<MetaverseSignup> MetaverseSignups { get; set; }
    }
}
