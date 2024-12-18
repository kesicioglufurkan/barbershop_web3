using System.ComponentModel.DataAnnotations;

namespace barbershop_web2.Models
{
    public class Saloon
    {
        public int SaloonID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Salon Adı")]
        public string SaloonName { get; set; }
        public ICollection<Employee>? Employees{ get; set; }

    }
}
