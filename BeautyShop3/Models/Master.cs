using System.ComponentModel.DataAnnotations;

namespace BeautyShop3.Models
{
    public class Master
    {
        public int id { get; set; }
        public string name { get; set; }
        public string masterImg { get; set; } = "/img/images.png";
    }
}
