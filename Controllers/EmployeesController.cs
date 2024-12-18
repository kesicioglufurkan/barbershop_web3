using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barbershop_web2.Models;
using Microsoft.AspNetCore.Authorization;

namespace barbershop_web2.Controllers
{
    public class EmployeeController : Controller
    {
        SaloonContext s = new SaloonContext();

        [Authorize(Roles = "Admin")]
        public IActionResult employeeCreate()
        {
            ViewBag.SaloonList = s.Saloons
            .Select(s => new SelectListItem
            {
                Value = s.SaloonID.ToString(),
                Text = s.SaloonName
            }).ToList();

            ViewBag.ServiceList = s.Services
            .Select(s => new SelectListItem
            {
                Value = s.ServiceID.ToString(),
                Text = s.ServiceName
            }).ToList();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult employeeCreate(Employee employee)
        {

           

                s.Employees.Add(employee);
                s.SaveChanges();
                TempData["msj"] = "Employee created successfully!";
                return RedirectToAction("Index","Home");
            

            TempData["msj"] = "Failed to create employee. Please check the inputs.";
            return View(employee);
        }
    }

}
