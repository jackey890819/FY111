namespace FY111.Models.Dto
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
}
