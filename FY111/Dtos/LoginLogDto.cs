using System;

namespace FY111.Dtos
{
    public class LoginLogDto
    {
        public string MemberId { get; set; }
        public int DeviceType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
