using BeautyShop3.Data;
using BeautyShop3.Models;
using BeautyShop3.PasswordHasher;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeautyShop3.Controllers
{
    public class LoginController : Controller
    {
        private readonly IPasswordHasherService passwordHasherService;
        private readonly BeautyShopDB context;
        public LoginController(BeautyShopDB context, IPasswordHasherService passwordHasherService)
        {
            this.passwordHasherService = passwordHasherService;
            this.context = context;
        }


        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string Phone, string Password)
        {
            var user = await context.users.FirstOrDefaultAsync(u => u.phone == Phone);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View("Index");
            }

            if (!passwordHasherService.VerifyPassword(user.password, Password))
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                return View("Index");
            }

            HttpContext.Session.SetString("UserLoggedIn", "true");
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));

            return RedirectToAction("Profile", "User");

        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            var checkPhone = context.users.FirstOrDefault(u => u.phone == model.Phone);

            if (checkPhone != null)
            {
                ModelState.AddModelError("Phone", "Пользователь с таким телефоном уже существует!");
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                context.users.Add(new BeautyShop3.Models.User
                {
                    phone = model.Phone,
                    name = model.Username,
                    password = passwordHasherService.HashPassword(model.Password)
                });
                context.SaveChanges();
                ModelState.Clear();

                ViewData["SuccessMessage"] = "Вы успешно зарегистрировались!";
            }

            return View("Index");
        }
    }
}
