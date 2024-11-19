using BeautyShop3.Data;
using BeautyShop3.Models;
using BeautyShop3.PasswordHasher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BeautyShop3.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {

        private readonly BeautyShopDB context;
        private readonly IPasswordHasherService passwordHasherService;

        public UserController(BeautyShopDB context, IPasswordHasherService passwordHasherService)
        {
            this.passwordHasherService = passwordHasherService;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        private Models.User GetUser()
        {
            var userSession = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userSession))
            {
                ViewData["user"] = null;
                return null;
            }

            try
            {
                var user = JsonConvert.DeserializeObject<Models.User>(userSession);
                ViewData["user"] = user;
                return user;
            }
            catch (JsonException ex)
            {
                ViewData["user"] = null;
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var user = GetUser();
            var orders = context.orders.ToList();
            var users = context.users.Join(orders, u => u.id, o => o.userId, (u, o) => u).ToListAsync();
            var userOrders = await context.users
            .Join(
                context.orders,
                u => u.id,          
                o => o.userId,
                (u, o) => new { u, o }
                )
                .Join(
                    context.services,
                    combined => combined.o.serviceId,
                    s => s.id,
                    (combined, s) => new { combined.u, combined.o, ServiceName = s.name }
                )
                .Join(
                context.masters,
                combined => combined.o.masterId, 
                m => m.id,
                (combined, m) => new  UserOrderViewModel           
                {
                    orderId = combined.o.id,
                    userName = combined.u.name,
                    serviceName = combined.ServiceName, 
                    masterName = m.name,                 
                    orderDate = DateTime.SpecifyKind(combined.o.DateTime, DateTimeKind.Utc)
                })
            .ToListAsync();

            return Json(userOrders);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewData.Clear();
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> AddService(string serviceName, string descriptionService, int priceService, int masterName)
        {
            GetUser();
            var masters = await context.masters.ToListAsync();
            ViewData["masters"] = masters;
            var addService = new BeautyShop3.Models.Service
            {
                name = serviceName,
                description = descriptionService,
                price = priceService
            };
            context.services.Add(addService);
            context.SaveChangesAsync();
            return View("AddService");
        }

        //public async Task<IActionResult> AddMaster()
        //{
        //    GetUser();
        //}
    }
}
