using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using barbershop_web3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace barbershop_web3.Controllers
{
    public class SaloonsController : Controller
    {
        SaloonContext s = new SaloonContext();


        // GET: Saloon/Create
        public IActionResult SaloonCreate()
        {
            return View();
        }

        // POST: Saloon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaloonCreate(Saloon saloon)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına yeni salon ekleniyor
                s.Saloons.Add(saloon);
                s.SaveChanges();

                // Başarılı bir şekilde eklendiği mesajını göster
                TempData["msj"] = "Salon başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index)); // İndex sayfasına yönlendir
            }
            else
            {
                TempData["msj"] = "Salon oluşturulamadı. Lütfen bilgileri kontrol edin.";
                return View(saloon); // Hata durumunda formu tekrar göster
            }
        }

        // GET: Saloon/Index (Salonlar listesi için)
        public IActionResult Index()
        {
            var saloons = s.Saloons.ToList(); // Tüm salonları getir
            return View(saloons); // Salonlar listesini View'a gönder
        }
    }
}
