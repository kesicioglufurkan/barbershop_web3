using barbershop_web2.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace barbershop_web2.Models
{
    public class Appointment
    {

        [Key]
        public int AppointmentID { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }
        public int EmployeeID { get; set; }
        public Employee? Employee { get; set; }

        public int ServiceID { get; set; }

        public int appointmentMoney { get; set; }
        public int appointmentHour { get; set; }

        [Required]
        [Display(Name = "Time")]

        public DateTime AppointmentTime { get; set; }
        public string AppointmentState { get; set; }


    }
}
