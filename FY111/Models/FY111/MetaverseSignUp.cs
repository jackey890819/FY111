using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class MetaverseSignUp
    {
        public string MemberId { get; set; }
        public int MetaverseId { get; set; }
        public DateTime? Time { get; set; }

        public virtual Metaverse Metaverse { get; set; }
    }
}
