using System.ComponentModel.DataAnnotations;

namespace barbershop_web3.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Personel Adı")]
        public string EmployeeName { get; set; }
        [Display(Name = "Salon")]
        public int SaloonID { get; set; }
        public Saloon Saloon { get; set; }
        public int? totalMoney { get; set; }
        public int? totelWorkHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Service>? Services { get; set; } // Çoklu ilişki
    }

}
