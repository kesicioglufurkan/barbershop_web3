using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barbershop_web3.Models;

namespace barbershop_web3.Controllers
{
    public class AppointmentsController : Controller
    {
        SaloonContext s = new SaloonContext();

        /**public IActionResult appoCreate()
                

        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                TempData["ErrorMessage"] = "User not authenticated. Please log in.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.UserIDL = int.Parse(userIdClaim); // Kullanıcı ID'sini ViewBag'e atıyoruz
            ViewBag.EmployeeList = new SelectList(s.Employees, "EmployeeID", "EmployeeName");
            ViewBag.ServiceList = new SelectList(s.Services, "ServiceID", "ServiceName");

            return View();
        } */

        [HttpGet]
        public JsonResult GetEmployeesBySaloon(int saloonId)
        {
            var employees = s.Employees
                .Where(e => e.SaloonID == saloonId)
                .Select(e => new
                {
                    e.EmployeeID,
                    e.EmployeeName
                })
                .ToList();

            return Json(employees);
        }
        [HttpGet]
        public JsonResult GetServicesByEmployee(int employeeId)
        {
            var services = s.Employees
                .Where(e => e.EmployeeID == employeeId)
                .SelectMany(e => e.Services)
                .Select(s => new
                {
                    s.ServiceID,
                    s.ServiceName
                })
                .ToList();

            return Json(services);
        }

        public IActionResult appoCreate()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                TempData["ErrorMessage"] = "User not authenticated. Please log in.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.UserIDL = int.Parse(userIdClaim); // Kullanıcı ID'sini ViewBag'e atıyoruz
            ViewBag.SaloonList = s.Saloons.ToList(); // Salon listesini ViewBag'e ekle

            return View();
        }

        [HttpPost]
        public IActionResult appoCreate(Appointment appointment)
        {
            appointment.AppointmentState = "0"; //Pending
            // Seçilen servis bilgilerini al
            var selectedService = s.Services
                    .FirstOrDefault(service => service.ServiceID == appointment.ServiceID);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                TempData["ErrorMessage"] = "User not authenticated. Please log in.";
                return RedirectToAction("Login", "Account");
            }

            if (selectedService == null)
            {
                TempData["ErrorMessage"] = "Service not found.";
                return RedirectToAction("Create");
            }

            // Yeni randevunun bitiş saatini hesapla
            DateTime appointmentEndTime = appointment.AppointmentTime.AddHours(selectedService.Time);

            // Çakışma kontrolü yap
            // Tüm servis bilgilerini ve mevcut randevuları al
            var appointments = s.Appointments.Where(a => a.EmployeeID == appointment.EmployeeID).ToList();
            var allServices = s.Services.ToList();

            // Çakışma kontrolü
            bool isTimeSlotAvailable = !appointments.Any(a =>
            {
                var existingService = allServices.FirstOrDefault(service => service.ServiceID == a.ServiceID);
                if (existingService == null) return false;

                // Mevcut randevunun bitiş saatini hesapla
                DateTime existingAppointmentEndTime = a.AppointmentTime.AddHours(existingService.Time);

                // Çakışma kontrolü
                return appointment.AppointmentTime < existingAppointmentEndTime &&
                       appointmentEndTime > a.AppointmentTime;
            });


            if (!isTimeSlotAvailable)
            {
                TempData["ErrorMessage"] = "This time slot is already occupied.";
                return RedirectToAction("appoCreate");
            }


            // Randevunun ücret ve süre bilgilerini ekle
            appointment.appointmentMoney = (int)selectedService.Price;
            appointment.appointmentHour = selectedService.Time;
            appointment.UserID = int.Parse(userIdClaim);
            // Yeni randevuyu kaydet
            s.Appointments.Add(appointment);
            s.SaveChanges();

            TempData["SuccessMessage"] = "Appointment created successfully!";
            return RedirectToAction("Index", "Home");


        }


    }
}
