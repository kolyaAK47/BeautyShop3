using BeautyShop3.Data;
using BeautyShop3.Models;
using BeautyShop3.PasswordHasher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BeautyShopDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);  // ����� �������� ������
    options.Cookie.HttpOnly = true;  // ����������, ����� �� cookie ���� �������� ����� JavaScript
    options.Cookie.IsEssential = true;  // ���������, ��� cookie �������� ������������
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // ��� ����������� UserController � ������������ ���� MyApp.Controllers.User
    endpoints.MapControllerRoute(
        name: "user",
        pattern: "User/{action=Index}/{id?}",
        defaults: new { controller = "User", area = "User" });

    // ��� ����������� UserController � ������������ ���� MyApp.Controllers.Admin
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{action=Index}/{id?}",
        defaults: new { controller = "User", area = "Admin" });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
