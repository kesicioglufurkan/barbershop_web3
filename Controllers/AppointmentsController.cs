using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barbershop_web2.Models;

namespace barbershop_web2.Controllers
{
    public class AppointmentsController : Controller
    {
        SaloonContext s = new SaloonContext();

        public IActionResult appoCreate()
        {
            ViewBag.EmployeeList = new SelectList(s.Employees, "EmployeeID", "EmployeeName");
            ViewBag.ServiceList = new SelectList(s.Services, "ServiceID", "ServiceName");

            return View();
        }

        [HttpPost]
        public IActionResult appoCreate(Appointment appointment)
        {
            appointment.AppointmentState = "0"; //Pending
            // Seçilen servis bilgilerini al
            var selectedService = s.Services
                    .FirstOrDefault(service => service.ServiceID == appointment.ServiceID);

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

                // Yeni randevuyu kaydet
                s.Appointments.Add(appointment);
                s.SaveChanges();

                TempData["SuccessMessage"] = "Appointment created successfully!";
                return RedirectToAction("Index","Home");


        }


    }
}
