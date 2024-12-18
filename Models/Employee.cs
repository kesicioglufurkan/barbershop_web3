using System.ComponentModel.DataAnnotations;

namespace barbershop_web2.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public int SaloonID { get; set; }     
        public Saloon Saloon { get; set; }
        public int ServiceID { get; set; }
        public Service Service { get; set; }
        public int? totalMoney { get; set; }
        public int? totelWorkHour { get; set; }
        
        
        public ICollection<Appointment>? Appointments { get; set; }

    }
}
