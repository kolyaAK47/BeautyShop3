using BeautyShop3.Data;
using BeautyShop3.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautyShop3.Controllers
{
    public class MasterController : Controller
    {
        private readonly BeautyShopDB context;
        public MasterController(BeautyShopDB context)
        {
            this.context = context;
        }

        public List<Master> GetAllMasters(BeautyShopDB context)
        {
            var masters = context.masters.ToList();
            return masters;
        }

        public IActionResult Index()
        {
            return View(GetAllMasters(this.context));
        }
    }
}
