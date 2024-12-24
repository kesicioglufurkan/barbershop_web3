using barbershop_web3.Models;
using System.ComponentModel.DataAnnotations;

namespace barbershop_web3.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Servis İsmi")]
        public string ServiceName { get; set; }
        [Required]
        [Display(Name = "Süre(saat)")]
        public int Time { get; set; }
        [Required]
        [Display(Name = "Ücret")]
        public int Price { get; set; }
        public ICollection<Employee> Employees { get; set; } // Çoklu ilişki
    }

}
