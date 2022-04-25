using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Member
    {
        public Member()
        {
            LoginLogs = new HashSet<LoginLog>();
            MetaverseLogs = new HashSet<MetaverseLog>();
            MetaverseSignIns = new HashSet<MetaverseSignIn>();
            MetaverseSignUps = new HashSet<MetaverseSignUp>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Avater { get; set; }
        public int Permission { get; set; }
        public byte State { get; set; }

        public virtual ICollection<LoginLog> LoginLogs { get; set; }
        public virtual ICollection<MetaverseLog> MetaverseLogs { get; set; }
        public virtual ICollection<MetaverseSignIn> MetaverseSignIns { get; set; }
        public virtual ICollection<MetaverseSignUp> MetaverseSignUps { get; set; }
    }
}
