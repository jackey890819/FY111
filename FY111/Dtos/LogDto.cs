using FY111.Models.FY111;
using System.Collections.Generic;

namespace FY111.Dtos
{
    public class AttendeeLogDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool checkin { get; set; }
        public AttendeeLogDto(string id, string name, bool checkin)
        {
            this.id = id;
            this.name = name;
            this.checkin = checkin;
        }
    }

    public class LogsDto
    {
        public List<OperationUnitLog> UnitLog { get; set; }
        public List<Class> @class { get; set; }
        public List<LoginLog> LoginLog { get; set; } 
    }
}
