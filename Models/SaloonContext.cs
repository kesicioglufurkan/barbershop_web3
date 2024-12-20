using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace barbershop_web3.Models
{
    public class SaloonContext:DbContext


    {

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Saloon> Saloons { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=Barber2024DB;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) //çoka çok kullanmak için
        {
            modelBuilder.Entity<EmployeeService>().HasNoKey();

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Services)
                .WithMany(s => s.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeService", // Ara tablo adı
                    join => join
                        .HasOne<Service>() // Her çalışan bir servise bağlı olabilir
                        .WithMany()
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Cascade), // Silinirse ilişkili veriyi de sil
                    join => join
                        .HasOne<Employee>() // Her servis bir çalışana bağlı olabilir
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade));
        }


    }
}
//Add-Migration InitialCreate
//Update-Database
//