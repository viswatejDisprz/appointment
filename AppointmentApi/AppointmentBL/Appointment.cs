using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AppointmentApi.AppointmentBL;

public class  Appointment
{
    [Required]
    public DateTime StartTime {get; set;}

    [Required]
    public DateTime EndTime {get; set;}

    [Required]
    public string?  Title {get; set;}

    [Required]
    public Guid Id {get; set;}
}