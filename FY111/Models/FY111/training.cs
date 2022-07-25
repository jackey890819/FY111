using System;
using System.Collections.Generic;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class training
    {
        public training()
        {
            ClassCheckins = new HashSet<ClassCheckin>();
            ClassSignups = new HashSet<ClassSignup>();
        }

        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ClassCheckin> ClassCheckins { get; set; }
        public virtual ICollection<ClassSignup> ClassSignups { get; set; }
    }
}
