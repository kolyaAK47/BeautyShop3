using BeautyShop3.Data;
using BeautyShop3.Models;
using BeautyShop3.PasswordHasher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Xml.Linq;

namespace BeautyShop3.Controllers
{
    public class UserController : Controller
    {
        private readonly BeautyShopDB context;
        private readonly IPasswordHasherService passwordHasherService;

        public UserController(BeautyShopDB context, IPasswordHasherService passwordHasherService)
        {
            this.passwordHasherService = passwordHasherService;
            this.context = context;
        }

        public async Task<IActionResult> GetAllOrders(int id)
        {
            var user = GetUser();
            var orders = context.orders.Where(o => o.userId == user.id).ToList();
            ViewData["orders"] = orders;


            return View("DefaultProfile");
        }


        [HttpGet]
        public async Task<IActionResult> GetMastersByService(int serviceId)
        {
            var masters = await context.ServiceMaster
                .Where(sm => sm.serviceId == serviceId)
                .Join(context.masters, sm => sm.masterId, m => m.id, (sm, m) => m)
                .ToListAsync();

            return Json(masters);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeSlots(int masterId)
        {
            var timeslots = await context.slots
                .Where(s => s.masterId == masterId && s.isBook != true)
                .ToListAsync();

            return Json(timeslots);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var user = GetUser();
            var services = await context.services.ToListAsync();

            ViewData["services"] = services;
            ViewData["masters"] = new List<Master>();
            ViewData["timeslots"] = new List<TimeSlot>();

            return View();
        }

        [HttpPost]
        public IActionResult AddOrder(int serviceId, int masterId, DateTime dateTime)
        {

            var user = GetUser();
            var order = new Order
            {
                userId = user.id,
                serviceId = serviceId,
                masterId = masterId,
                DateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
            };
            var dt = context.slots.FirstOrDefault(s => s.date == dateTime);
            dt.isBook = true;
            context.orders.Add(order);
            context.SaveChanges();

            return View("DefaultProfile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewData.Clear();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            GetUser();
            var delOrder = await context.orders.FirstOrDefaultAsync(o => o.id == orderId);
            context.orders.Remove(delOrder);
            await context.SaveChangesAsync();
            GetAllOrders(GetUser().id);
            return View("DefaultProfile");
        }

        private User GetUser()
        {
            var userSession = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userSession))
            {
                ViewData["user"] = null;
                return null;
            }

            try
            {
                var user = JsonConvert.DeserializeObject<User>(userSession);
                ViewData["user"] = user;
                return user;
            }
            catch (JsonException ex)
            {
                ViewData["user"] = null;
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(int id, string name, string phone, string password)
        {
            GetUser();
            var upUser = await context.users.FirstOrDefaultAsync(u => u.id == id);
            var checkPhoneUser = await context.users.FirstOrDefaultAsync(u => u.phone == phone);
            if (checkPhoneUser != null && checkPhoneUser.id != id)
            {
                ModelState.AddModelError("phone", "Пользователь с таким телефоном уже существует!");
                return View("DefaultProfile");
            }
            if (upUser != null)
            {
                upUser.name = name;
                upUser.phone = phone;
                if (!string.IsNullOrEmpty(password))
                {
                    upUser.password = passwordHasherService.HashPassword(password);

                }

                await context.SaveChangesAsync();

                ViewData["user"] = upUser;
                ViewData["SuccessMessage"] = "Данные успешно изменены!";
                ViewData["isEditingProfile"] = false;

                return View("DefaultProfile");
            }

            return View("DefaultProfile");
        }

        [HttpGet]
        public IActionResult DeleteUser()
        {
            var user = GetUser();
            if (user != null)
            {
                context.users.Remove(user);
                context.SaveChanges();
                return RedirectToAction("Index", "Login");
            }

            return View("DefaultProfile");
        }

        [HttpGet]
        public IActionResult ChangeEditing()
        {
            ViewData["user"] = GetUser();
            ViewData["isEditingProfile"] = true;
            return View("DefaultProfile");
        }

        public IActionResult Profile()
        {
            if (GetUser() is null)
            {
                ModelState.AddModelError("", "Пользователь не авторизован");
                return RedirectToAction("Index", "Login");
            }
            if (GetUser().role == "admin")
            {
                return View("~/Areas/Admin/Views/User/AdminProfile.cshtml");
            }
            return View("DefaultProfile");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
