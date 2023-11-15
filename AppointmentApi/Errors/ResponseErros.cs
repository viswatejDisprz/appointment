using AppointmentApi.Models;

public static class ResponseErrors
{

    public static CustomError BadRequest(string message) => new CustomError { Message = message };

    public static CustomError ConflictError(string appointmentTime, DateTime startTime, DateTime endTime) =>
     new CustomError {Message = $"{appointmentTime} conflicting with an existing appointment having StartTime:" +
                        $"{startTime} and EndTime: {endTime}"};

   
   /////// validation errors
   
   public static string NotEmpty() => $"Appointment Request Title should not be empty";

   public static string GreaterThanCurrentTime(string s) => $"Appointment {s} should be greater than Current Time";

   public static string EndtimeGreaterThanStartTime() => $"End Time must be greater than start time";

   public static string SameDay() => "StartTime and EndTime should have the same day";

   public static string DateFormatError() => "Please enter the Correct date format in : MM/dd/yyyy"; 

}