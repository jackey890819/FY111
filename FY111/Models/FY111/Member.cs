using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Member
    {
        public Member()
        {
            FriendMemberId1Navigations = new HashSet<Friend>();
            FriendMembers = new HashSet<Friend>();
            MemberHasDevices = new HashSet<MemberHasDevice>();
            MemberHasGroups = new HashSet<MemberHasGroup>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Avater { get; set; }
        public int Permission { get; set; }

        public virtual ICollection<Friend> FriendMemberId1Navigations { get; set; }
        public virtual ICollection<Friend> FriendMembers { get; set; }
        public virtual ICollection<MemberHasDevice> MemberHasDevices { get; set; }
        public virtual ICollection<MemberHasGroup> MemberHasGroups { get; set; }
    }
}
