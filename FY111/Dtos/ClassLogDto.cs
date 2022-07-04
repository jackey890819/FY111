using System;

namespace FY111.Dtos
{
    public class ClassLogDto
    {
        public string MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Score { get; set; }

    }

    
}
