﻿using barbershop_web2.Models;
using System.ComponentModel.DataAnnotations;

namespace barbershop_web2.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Service name")]
        public string ServiceName { get; set; }
        [Required]
        [Display(Name = "Time (hour)")]
        public int Time { get; set; }
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}