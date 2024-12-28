using Microsoft.AspNetCore.Authentication.Cookies;
using barbershop_web3.Controllers;

namespace barbershop_web3
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews();


            // Authentication ve Authorization hizmetlerini ekliyoruz
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login"; // Login sayfas�n�n yolu
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // HSTS ayarlar�
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();

            // Authentication ve Authorization middleware'lerini ekliyoruz
            app.UseAuthentication(); // Kimlik do�rulama i�lemi
            app.UseAuthorization();  // Yetkilendirme i�lemi

            // Rota ayar�
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
