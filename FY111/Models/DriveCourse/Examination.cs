using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.DriveCourse
{
    public partial class Examination
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public short Score { get; set; }
        public string Remark { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
