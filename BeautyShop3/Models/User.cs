

namespace BeautyShop3.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string? role { get; set; } = "User";

    }
}
