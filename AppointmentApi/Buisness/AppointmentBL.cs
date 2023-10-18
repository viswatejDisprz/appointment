using System.Globalization;
using System.Text.RegularExpressions;
using AppointmentApi.DataAccess;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Authentication;
namespace AppointmentApi.Buisness
{
   public class AppointmentBL:IAppointmentBL
   {
      private readonly IAppointmentDL appointmentDL;

      public AppointmentBL(IAppointmentDL appointmentDL)
      {
        this.appointmentDL = appointmentDL;
      }


      // next change here
    //   public IEnumerable<Appointment> GetAppointments(){
    //        var SortedAppointments = appointmentDL.GetAppointments().OrderBy(appointment => appointment.StartTime);
    //        return SortedAppointments;
    //   }

      
      // This function fetches appointment by date
      public List<Appointment> GetAppointments(Guid? id,DateOnly? date) // GEtAppointments date change the date to custom ApointmentDateRequest dto new and add validator
      {
             ///// removes all these push it to validationes only single line
                    // to check if the entered date format is correct or not
                    // string regexPattern = @"^\d{2}-\d{2}-\d{4}$";
                    // Regex regex = new Regex(regexPattern);
                    // if(!regex.Match(date.ToString()).Success)
                    // {
                    //     return null;
                    // }

                    // Filter appointments by date
                    if(date is not null){
                    List<Appointment> filteredAppointments = appointmentDL.GetAppointments(null,date);
                    // TimeOnly timeOnly = new TimeOnly(12, 30, 0);
                    // DateTime dt;
                    // if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    // {
                    //     Console.WriteLine("The date is valid.");
                    //     DateOnly dateOnly = new DateOnly(dt.Year, dt.Month, dt.Day);
                    //     var appointments = appointmentDL.GetAppointments(null,dateOnly);
                    //     foreach(var item in appointments)
                    //     {
                    //             if(item.StartTime.Date == dt.Date)
                    //             {
                    //                 filteredAppointments.Add(item);
                    //             }
                    //     }
                        return filteredAppointments.OrderBy(app => app.StartTime).ToList();
                    }
                    else if(id is not null)
                    {
                         List<Appointment> filteredAppointments = appointmentDL.GetAppointments(id,null);
                         return filteredAppointments.OrderBy(app => app.StartTime).ToList();
                    }
                    else
                    {
                         List<Appointment> filteredAppointments = appointmentDL.GetAppointments();
                         return filteredAppointments.ToList();
                    }
                    }
                    // else
                    // {
                    //     Console.WriteLine("The date is not valid.");
                    //     return null;
                    // }

      /// not required
      
    //   public Appointment GetAppointment(Guid id){
    //     return appointmentDL.GetAppointments().Where(appointment => appointment.Id == id).SingleOrDefault();
    //   }

    //   Http response exception  try to use this last change

      //Funtion to create appointment
      public string CreateAppointment(AppointmentRequest appointmentrequest){
                

                //checking appointment with validator
                AppointmentRequestValidator validator = new AppointmentRequestValidator();
                FluentValidation.Results.ValidationResult results = validator.Validate(appointmentrequest);

                if (!results.IsValid)
                { 
                    string endTime = "greater";
                    foreach (var failure in results.Errors)
                    {
                        if(failure.ErrorMessage.IndexOf(endTime, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            return "End time must be greater than Start Time.";
                        }
                        Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    }
                    return "Input Invalid";
                }

                // Convert the dto in main model object
                Appointment appointment = new Appointment
                {
                    StartTime = appointmentrequest.StartTime,
                    EndTime = appointmentrequest.EndTime,
                    Title = appointmentrequest.Title,
                    Id = Guid.NewGuid()
                };
                

                // check for same date of appointment and if startTime == endTime
                if((appointmentrequest.StartTime == appointmentrequest.EndTime) || (appointmentrequest.StartTime.Date != appointmentrequest.EndTime.Date)){
                        return null;
                }

                DateOnly dateOnly = new DateOnly(appointmentrequest.StartTime.Year, appointmentrequest.StartTime.Month, appointmentrequest.StartTime.Day);
 
                var appointments = appointmentDL.GetAppointments(null,dateOnly);

            
            // 409 response when there is a conflict
                // var conflictingAppointments = appointments.Where(item =>
                //     item.StartTime.Date == appointment.EndTime.Date && item.EndTime.Date == appointment.EndTime.Date);

                if (appointments.Any(item =>
                    item.StartTime < appointmentrequest.StartTime && item.EndTime > appointmentrequest.StartTime))
                    {
                        var conflictingAppointment = appointments.First(item =>
                            item.StartTime < appointmentrequest.StartTime && item.EndTime > appointmentrequest.StartTime);
                        var errorString = appointmentrequest.StartTime + " is conflicting with an existing appointment having startTime: " +
                            conflictingAppointment.StartTime + " and endTime: " + conflictingAppointment.EndTime;
                        return errorString;
                    }

                if (appointments.Any(item =>
                    appointmentrequest.EndTime > item.StartTime && appointmentrequest.EndTime < item.StartTime))
                    {
                        var conflictingAppointment = appointments.First(item =>
                            appointmentrequest.EndTime > item.StartTime && appointmentrequest.EndTime < item.StartTime);
                        var errorString = appointmentrequest.EndTime + " is conflicting with an existing appointment having startTime: " +
                            conflictingAppointment.StartTime + " and endTime: " + conflictingAppointment.EndTime;
                        return errorString;
                    }

                var stringId = appointmentDL.CreateAppointment(appointment);

                return stringId.ToString();
            
      }

        //funtion to delete an appointment
        public bool DeleteAppointment(Guid id)
        { 
            var appointments  = appointmentDL.GetAppointments(id,null);
            if(appointments.Count == 0)
            {
                return false;
            }
            appointmentDL.DeleteAppointment(id);
            return true;
        }
    }
}