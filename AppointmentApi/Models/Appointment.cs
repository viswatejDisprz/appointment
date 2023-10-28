// using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AppointmentApi.Models;

public class  Appointment
{

    public DateTime StartTime {get; set;}

    public DateTime EndTime {get; set;}

    public string?  Title {get; set;}
    
    public Guid Id {get; set;}
}