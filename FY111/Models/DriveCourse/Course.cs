using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.DriveCourse
{
    public partial class Course
    {
        public Course()
        {
            CourseMembers = new HashSet<CourseMember>();
            Examinations = new HashSet<Examination>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public short? NumberOfPeople { get; set; }
        public string CourseOutline { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<CourseMember> CourseMembers { get; set; }
        public virtual ICollection<Examination> Examinations { get; set; }
    }
}
