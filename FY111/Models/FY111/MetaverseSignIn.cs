using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class MetaverseSignIn
    {
        public string MemberId { get; set; }
        public int MetaverseId { get; set; }

        public virtual Metaverse Metaverse { get; set; }
    }
}
