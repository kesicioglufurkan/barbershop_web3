using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barbershop_web3.Models;
using Microsoft.AspNetCore.Authorization;

namespace barbershop_web3.Controllers
{
    public class EmployeeController : Controller
    {
        SaloonContext s = new SaloonContext();

      

        [Authorize(Roles = "Admin")]
        // Employee Create GET Method
        public IActionResult employeeCreate()
        {
            // Saloon Listesi
            ViewBag.SaloonList = new SelectList(s.Saloons, "SaloonID", "SaloonName");

            // Service Listesi
            var serviceList = s.Services.Select(service => new SelectListItem
            {
                Value = service.ServiceID.ToString(),
                Text = service.ServiceName
            }).ToList();

            ViewBag.ServiceList = serviceList;

            // Eğer servisler boşsa, hata mesajı gösterebilirsiniz
            if (serviceList.Count == 0)
            {
                TempData["msj"] = "No services available. Please add services first.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Employee Create POST Method
        public IActionResult employeeCreate(Employee employee, int[] SelectedServices)
        {

                // Yeni çalışan kaydı
                s.Employees.Add(employee);
                s.SaveChanges();

                // Çalışana seçilen hizmetleri ekleme
                foreach (var serviceId in SelectedServices)
                {
                    employee.Services ??= new List<Service>();
                    var service = s.Services.FirstOrDefault(s => s.ServiceID == serviceId);
                    if (service != null)
                    {
                        employee.Services.Add(service);
                    }
                }

                s.SaveChanges();

                TempData["msj"] = "Employee created successfully!";
                return RedirectToAction("Index");
;
        }
    }

}
