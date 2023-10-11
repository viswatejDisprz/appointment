using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Models;

public class AppointmentContext : DbContext
{
    public AppointmentContext(DbContextOptions<AppointmentContext> options) :base(options)
    {
        
    }

    public DbSet<Appointment> Appointment{get; set;}
}