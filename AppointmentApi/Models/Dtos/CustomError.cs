// This is a custom error dto to deetect Bad Requests
namespace AppointmentApi.Models;

public class CustomError:IResponseDto
{
    public string Message{get; set;} 
}