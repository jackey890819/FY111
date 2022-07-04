namespace FY111.Dtos
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public byte SignupEnabled { get; set; }
        public byte CheckinEnabled { get; set; }
        public int? Duration { get; set; }
    }


}