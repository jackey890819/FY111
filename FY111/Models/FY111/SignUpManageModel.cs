namespace FY111.Models.FY111
{
    public class SignUpManageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public int? Duration { get; set; }
        public bool isSignedUp { get; set; }
    }
}
