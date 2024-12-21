using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using barbershop_web3.Models;
using Microsoft.AspNetCore.Authorization;

namespace barbershop_web3.Controllers
{
    public class UsersController : Controller
    {
        SaloonContext s = new SaloonContext();

        [Authorize(Roles = "Admin")] // Sadece Admin rolüne izin veriliyor
        public IActionResult AdminPanel()
        {

            var appointments = s.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .ToList(); // İlişkili verilerle birlikte tüm randevuları al.
            return View(appointments);
        }


        [Authorize]
        public IActionResult Index()
        {
            var users = s.Users.ToList();
            return View(users);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = s.Users
                .FirstOrDefault(u => u.UserNick == user.UserNick && u.UserPass == user.UserPass);

            if (existingUser != null)
            {
                // Rol belirleme: Admin mi yoksa User mı?
                var role = (user.UserNick == "admin" && user.UserPass == "sau")
                    ? "Admin"
                    : "User";

                var claims = new List<Claim>
            {

            new Claim(ClaimTypes.Name, existingUser.UserNick),
            new Claim(ClaimTypes.NameIdentifier, existingUser.UserID.ToString()), // Kullanıcı ID'si
            new Claim(ClaimTypes.Role, role) // Role ekleniyor
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                TempData["msj"] = "Login Successful!";

                // Admin kullanıcıysa Admin paneline yönlendirme
                if (role == "Admin")
                {
                    return RedirectToAction("AdminPanel");
                }



                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["msj"] = "Invalid username or password!";
                return RedirectToAction("Login","Users");
            }
        }

        public IActionResult Logout()
        {
            // Kullanıcının oturumunu sonlandır
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Kullanıcıyı Login sayfasına yönlendir
            return RedirectToAction("Login");
        }


  
        public IActionResult userAdd()
        {
            return View();
        }


        [HttpPost]
        public IActionResult userSave(User user)
        {
            
                s.Users.Add(user);
                s.SaveChanges();
                TempData["msj"] = "User eklendi";
                return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult UpdateState(int id)
        {
            // Randevuyu yükle
            var appointment = s.Appointments.FirstOrDefault(a => a.AppointmentID == id);

            if (appointment != null)
            {
                appointment.AppointmentState = "1"; // Durumu güncelle

                // Service tablosundan fiyat ve süre bilgilerini al
                var service = s.Services.FirstOrDefault(s => s.ServiceID == appointment.ServiceID);

                if (service != null)
                {
                    // Çalışanın toplam parasını ve çalışma saatini güncelle
                    var employee = s.Employees.FirstOrDefault(e => e.EmployeeID == appointment.EmployeeID);
                    if (employee != null)
                    {
                        employee.totalMoney += service.Price; // Servis fiyatını ekle
                        employee.totelWorkHour += service.Time; // Servis süresini ekle
                    }

                    TempData["SuccessMessage"] = "Appointment state and employee data updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Service not found!";
                }

                s.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Appointment not found!";
            }

            return RedirectToAction("AdminPanel");
        }
        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserDelete(int id)
        {
            var user = s.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                s.Users.Remove(user);
                s.SaveChanges();
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found!";
            }

            return RedirectToAction("Index");
        }



    }
}
