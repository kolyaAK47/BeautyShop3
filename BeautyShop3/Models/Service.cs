using System.ComponentModel.DataAnnotations;

namespace BeautyShop3.Models
{
    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public int price { get; set; }
    }
}
