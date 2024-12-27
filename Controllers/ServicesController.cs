using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using barbershop_web3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace barbershop_web3.Controllers
{
    public class ServicesController : Controller
    {
        SaloonContext s = new SaloonContext();

        public IActionResult Index()
        {
            var services = s.Services.ToList();
            return View(services);
        }


        // GET: Service/Create
        [Authorize(Roles = "Admin")]
        public IActionResult serviceCreate()
        {
            // Eğer varsa, salon listesi eklenebilir
            ViewBag.SaloonList = s.Saloons
                .Select(s => new SelectListItem
                {
                    Value = s.SaloonID.ToString(),
                    Text = s.SaloonName
                }).ToList();

            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult serviceCreate(Service service)
        {
            
                // Servisi veritabanına ekle
                s.Services.Add(service);
                s.SaveChanges();

                // Başarı mesajı
                TempData["SuccessMessage"] = "Service created successfully!";
                return RedirectToAction("Index", "Services");


        }

        public IActionResult serviceEdit(int id)
        {
            var service = s.Services.FirstOrDefault(s => s.ServiceID == id);

            if (service == null)
            {
                TempData["ErrorMessage"] = "Service not found!";
                return RedirectToAction("Index");
            }

            return View(service);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult serviceEdit(Service service)
        {

                // Veritabanında güncellenmek istenen servisi bulma
                var existingService = s.Services.FirstOrDefault(s => s.ServiceID == service.ServiceID);

                if (existingService == null)
                {
                    // Servis bulunamazsa hata mesajı
                    TempData["ErrorMessage"] = "Service not found!";
                    return RedirectToAction("Index");
                }

                // Veritabanındaki servis bilgilerini güncelleme
                existingService.ServiceName = service.ServiceName;
                existingService.Time = service.Time;
                existingService.Price = service.Price;

                // Değişiklikleri kaydetme
                s.SaveChanges();

                // Başarı mesajı
                TempData["SuccessMessage"] = "Service updated successfully!";
                return RedirectToAction("Index");

        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult serviceDelete(int id)
        {
            var service = s.Services.FirstOrDefault(s => s.ServiceID == id);
            if (service != null)
            {
                s.Services.Remove(service);
                s.SaveChanges();
                
            }
            else
            {
                TempData["ErrorMessage"] = "Service not found!";
            }

            return RedirectToAction("Index");
        }

    }
}
