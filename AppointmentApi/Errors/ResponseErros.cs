using AppointmentApi.Models;

public static class ResponseErrors
{
    public static CustomError NotFound => new CustomError { Message = "Appointment not found" };

    public static CustomError BadRequest(string message) => new CustomError { Message = message };

    public static CustomError ConflictError(DateTime appointmentTime, DateTime startTime, DateTime endTime) =>
     new CustomError {Message = $"{appointmentTime} is conflicting with an existing appointment having startTime:" +
                        $"{startTime} and endTime: {endTime}"};

}