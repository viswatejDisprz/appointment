using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
namespace AppointmentApi.AppointmentDL
{
    public class  AppointmentDto:ResponseDto
{
    public DateTime StartTime {get; set;}

    public DateTime EndTime {get; set;}

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
            if(EndTime < StartTime)
            {
                return true;
            }
            return false;
        }
}
}