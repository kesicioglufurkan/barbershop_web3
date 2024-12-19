using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using barbershop_web3.Models;
using Microsoft.AspNetCore.Authorization;

namespace barbershop_web3.Controllers
{
    public class ServicesController : Controller
    {
        SaloonContext s = new SaloonContext();

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
        [ValidateAntiForgeryToken]
        public IActionResult serviceCreate(Service service)
        {
            
                // Servisi veritabanına ekle
                s.Services.Add(service);
                s.SaveChanges();

                // Başarı mesajı
                TempData["SuccessMessage"] = "Service created successfully!";
                return RedirectToAction("Index", "Home");
            
            
        }
    }
}
