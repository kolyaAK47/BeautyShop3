using BeautyShop3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BeautyShop3.Data
{
    public class BeautyShopDB : DbContext
    {
        public BeautyShopDB(DbContextOptions<BeautyShopDB> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Master> masters { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<TimeSlot> slots { get; set; }
        public DbSet<ServiceMaster> ServiceMaster { get; set; }
    }
}
