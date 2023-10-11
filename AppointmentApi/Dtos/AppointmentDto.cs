using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
namespace AppointmentApi.Dtos
{
    public class  AppointmentDto:IPostDto
{
    [Required]
    public DateTime StartTime {get; set;}

    [Required]
    public DateTime EndTime {get; set;}

    [Required]
    public string  Title {get; set;}

    // public Guid Id {get; set;}
    public bool IsValid()
        {
            if(Title.IsNullOrEmpty()){
                return true;
            }
            if(StartTime <= DateTime.MinValue && StartTime >= DateTime.MaxValue)
            {
                return true;
            }
            if(EndTime <= DateTime.MinValue && EndTime >= DateTime.MaxValue)
            {
                return true;
            }
            return false;
        }
}
}