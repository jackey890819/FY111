using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class ClassQuestion
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Discription { get; set; }
        public string Option { get; set; }

        public virtual Class Class { get; set; }
    }
}
