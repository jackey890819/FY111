using System;

namespace FY111.Dtos
{
    public class ClassCheckinDto
    {
        public string MemberId { get; set; }
        public int ClassId { get; set; }
        public DateTime? Time { get; set; }
    }
}
