using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.DriveCourse
{
    public partial class DriveCourseUser
    {
        public DriveCourseUser()
        {
            CourseMembers = new HashSet<CourseMember>();
            Examinations = new HashSet<Examination>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PersonalImg { get; set; }
        public byte Permission { get; set; }

        public virtual ICollection<CourseMember> CourseMembers { get; set; }
        public virtual ICollection<Examination> Examinations { get; set; }
    }
}
