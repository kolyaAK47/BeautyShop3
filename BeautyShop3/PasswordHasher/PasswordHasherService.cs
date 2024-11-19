using Microsoft.AspNetCore.Identity;

namespace BeautyShop3.PasswordHasher
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> passwordHasher = new PasswordHasher<object>();

        // Хэширование пароля
        public string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }

        // Проверка пароля
        public bool VerifyPassword(string hashedPassword, string passwordToCheck)
        {
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, passwordToCheck);
            return result == PasswordVerificationResult.Success;
        }
    }
}
