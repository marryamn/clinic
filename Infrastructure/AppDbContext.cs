using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : SoftDeletes.Core.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }
        
        public DbSet<Doctor> Doctors { get; set; } 
        public DbSet<AppointmentPay?> AppointmentPays { get; set; } 
        public DbSet<Appointment> Appointments { get; set; } 
        public DbSet<Insurance?> Insurances { get; set; } 
        public DbSet<Patient?> Patients { get; set; } 
      
        
    }
}