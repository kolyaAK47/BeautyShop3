namespace BeautyShop3.Models
{
    public class Order
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public int masterId { get; set; }
        public int serviceId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
