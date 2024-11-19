using BeautyShop3.Data;
using BeautyShop3.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautyShop3.Controllers
{
    public class ServiceController : Controller
    {
        private readonly BeautyShopDB context;
        public ServiceController(BeautyShopDB context)
        {
            this.context = context;
        }

        public List<Service> GetAllServices(BeautyShopDB context)
        {
            var services = context.services.ToList();
            return services;
        }

        public IActionResult Index()
        {
            var services = context.services.ToList();
            ViewData["services"] = services;
            return View();
        }
    }
}
