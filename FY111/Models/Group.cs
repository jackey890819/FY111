using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models
{
    public partial class Group
    {
        public Group()
        {
            MemberHasGroups = new HashSet<MemberHasGroup>();
        }

        public string Name { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<MemberHasGroup> MemberHasGroups { get; set; }
    }
}
