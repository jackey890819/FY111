using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.DriveCourse
{
    public partial class CourseMember
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
        public byte? Type { get; set; }
        public string Remark { get; set; }

        public virtual Course Course { get; set; }
        public virtual DriveCourseUser User { get; set; }
    }
}
