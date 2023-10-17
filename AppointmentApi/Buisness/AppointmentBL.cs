using System.Globalization;
using System.Text.RegularExpressions;
using AppointmentApi.DataAccess;
using AppointmentApi.Models;
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
      public IEnumerable<Appointment> GetAppointments(){
           var SortedAppointments = appointmentDL.GetAppointments().OrderBy(appointment => appointment.StartTime);
           return SortedAppointments;
      }

      
      // This function fetches appointment by date
      public IEnumerable<Appointment> GetAppointmentsBydate(string date) // GEtAppointments date change the date to custom ApointmentDateRequest dto new and add validator
      {
             ///// removes all these push it to validationes only single line
                    // to check if the entered date format is correct or not
                    string regexPattern = @"^\d{2}-\d{2}-\d{4}$";
                    Regex regex = new Regex(regexPattern);
                    if(!regex.Match(date.ToString()).Success)
                    {
                        return null;
                    }

                    // Filter appointments by date
                    List<Appointment> filteredAppointments = new List<Appointment>();
                    TimeOnly timeOnly = new TimeOnly(12, 30, 0);
                    DateTime dt;
                    if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        Console.WriteLine("The date is valid.");
                        DateOnly dateOnly = new DateOnly(dt.Year, dt.Month, dt.Day);
                        var appointments = appointmentDL.GetAppointments(null,dateOnly);
                        foreach(var item in appointments)
                        {
                                if(item.StartTime.Date == dt.Date)
                                {
                                    filteredAppointments.Add(item);
                                }
                        }
                        return filteredAppointments.OrderBy(app => app.StartTime);
                    }
                    else
                    {
                        Console.WriteLine("The date is not valid.");
                        return null;
                    }
                    
      }

      /// not required
      
      public Appointment GetAppointment(Guid id){
        return appointmentDL.GetAppointments().Where(appointment => appointment.Id == id).SingleOrDefault();
      }

    //   Http response exception  try to use this last change

      //Funtion to create appointment
      public string CreateAppointment(AppointmentDto appointmentDto){
                
                // Put all this in validator
                if(appointmentDto.EndTime <= appointmentDto.StartTime )
                {
                    return "End time cannot be less than or equal to Start Time";
                }
                // Convert the dto in main model object
                Appointment appointment = new Appointment
                {
                    StartTime = appointmentDto.StartTime,
                    EndTime = appointmentDto.EndTime,
                    Title = appointmentDto.Title,
                    Id = Guid.NewGuid()
                };
                
                //checking appointment with validator
                AppointmentValidator validator = new AppointmentValidator();
                FluentValidation.Results.ValidationResult results = validator.Validate(appointment);

                if (!results.IsValid)
                {
                    foreach (var failure in results.Errors)
                    {
                        Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    }
                    return "Input Invalid";
                }

                // check for same date of appointment and if startTime == endTime
                if((appointment.StartTime == appointment.EndTime) || (appointment.StartTime.Date != appointment.EndTime.Date)){
                        return null;
                }

                DateOnly dateOnly = new DateOnly(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day);
 
                var appointments = appointmentDL.GetAppointments(null,dateOnly);

            
            // 409 response when there is a conflict
                // var conflictingAppointments = appointments.Where(item =>
                //     item.StartTime.Date == appointment.EndTime.Date && item.EndTime.Date == appointment.EndTime.Date);

                if (appointments.Any(item =>
                    item.StartTime < appointment.StartTime && item.EndTime > appointment.StartTime))
                    {
                        var conflictingAppointment = appointments.First(item =>
                            item.StartTime < appointment.StartTime && item.EndTime > appointment.StartTime);
                        var errorString = appointment.StartTime + " is conflicting with an existing appointment having startTime: " +
                            conflictingAppointment.StartTime + " and endTime: " + conflictingAppointment.EndTime;
                        return errorString;
                    }

                if (appointments.Any(item =>
                    appointment.EndTime > item.StartTime && appointment.EndTime < item.StartTime))
                    {
                        var conflictingAppointment = appointments.First(item =>
                            appointment.EndTime > item.StartTime && appointment.EndTime < item.StartTime);
                        var errorString = appointment.EndTime + " is conflicting with an existing appointment having startTime: " +
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