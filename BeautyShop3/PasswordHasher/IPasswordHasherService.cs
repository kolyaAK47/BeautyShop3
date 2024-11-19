namespace BeautyShop3.PasswordHasher
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string passwordToCheck);
    }
}
