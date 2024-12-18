using Microsoft.AspNetCore.Authentication.Cookies;

namespace barbershop_web2
{
    public class Program
    {
        // ConfigureServices metodunu kald�r�yoruz ��nk� .NET 6 ve sonras� i�in direkt olarak builder �zerinden yap�land�rma yap�l�yor
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
