namespace barbershop_web3.Models
{

        public class EmployeeService
        {
            public int EmployeeID { get; set; }
            public Employee Employee { get; set; }

            public int ServiceID { get; set; }
            public Service Service { get; set; }
        }
    
}
