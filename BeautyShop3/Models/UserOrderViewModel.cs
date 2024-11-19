namespace BeautyShop3.Models
{
    public class UserOrderViewModel
    {
        public int orderId { get; set; }
        public string userName { get; set; }
        public string serviceName { get; set; }
        public string masterName { get; set; }
        public DateTime orderDate { get; set; }
    }
}
