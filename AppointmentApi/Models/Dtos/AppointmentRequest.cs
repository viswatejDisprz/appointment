namespace AppointmentApi.Models;
public class  AppointmentRequest
{
    public DateTime StartTime {get; set;}

    public DateTime EndTime {get; set;}

    public string?  Title {get; set;}
}