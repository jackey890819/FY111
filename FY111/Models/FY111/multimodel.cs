namespace FY111.Models.FY111
{
    public partial class App_Login_Model
    {
        public Member member { get; set; }
        public int device_type { get; set; }
    }

    public partial class ListAvailable_Model
    {
        public int member_id { get; set; }
        public int permission { get; set; }
    }
}
