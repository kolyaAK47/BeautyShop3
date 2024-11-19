using System.Diagnostics.Contracts;

namespace BeautyShop3.Models
{
    public class TimeSlot
    {
        public int id {  get; set; }
        public int masterId { get; set; }
        public bool isBook { get; set; }
        public DateTime date { get; set; }
    }
}
