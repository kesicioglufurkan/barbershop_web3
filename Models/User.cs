using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace barbershop_web3.Models
{
    public class User
    {

        public int UserID { get; set; }

        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Kullanıcı Adı")]
        public string UserNick { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Şifre")]
        public string UserPass { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
