using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace barbershop_web2.Models
{
    public class User
    {

        public int UserID { get; set; }

        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "User nick")]
        public string UserNick { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "User Sifre")]
        public string UserPass { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
