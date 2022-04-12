using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Group
    {
        public Group()
        {
            MemberHasGroups = new HashSet<MemberHasGroup>();
        }

        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MemberHasGroup> MemberHasGroups { get; set; }
    }
}
