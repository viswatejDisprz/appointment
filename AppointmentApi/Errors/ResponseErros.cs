using AppointmentApi.Models;

public static class ResponseErrors
{
    public static CustomError NotFound => new CustomError { Message = "Appointment not found" };

    public static CustomError BadRequest(string message) => new CustomError { Message = message };

}