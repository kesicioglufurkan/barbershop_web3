using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using barbershop_web2.Models;
using Microsoft.AspNetCore.Authorization;

namespace barbershop_web2.Controllers
{
    public class UsersController : Controller
    {
        SaloonContext s = new SaloonContext();

        [Authorize(Roles = "Admin")] // Sadece Admin rolüne izin veriliyor
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            var users = s.Users.ToList();
            return View(users);
        }

        public IActionResult Login()
        {
           

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

                return RedirectToAction("Index");
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


        [Authorize]
        public IActionResult userAdd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult userSave(User user)
        {
            if (ModelState.IsValid)
            {
                s.Users.Add(user);
                s.SaveChanges();
                TempData["msj"] = "User eklendi";
                return RedirectToAction("Index");
            }

            TempData["msj"] = "Lütfen Dataları düzgün giriniz";
            return RedirectToAction("userAdd");
        }


    }
}
