using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

      public IEnumerable<Appointment> GetAppointments(){
           var SortedAppointments = appointmentDL.GetAppointments().OrderBy(appointment => appointment.StartTime);
           return SortedAppointments;
      }

      
      // This function fetches appointment by date
      public IEnumerable<Appointment> GetAppointmentsBydate(string date)
      {
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
                    //  = DateTime.ParseExact(date, "dd-MM-yyyy", null);
                    // string x = "15-10-2023";
                    // DateTime dt;
                    if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        Console.WriteLine("The date is valid.");
                        var appointments = appointmentDL.GetAppointments();
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
                    
                    // if (dt.Month > 12 || dt.Day > 31)
                    // {
                    //     return null;
                    // }else{
                    //     // date.ToDateTime(timeOnly);
                    //     var appointments = appointmentDL.GetAppointments();
                    //     foreach(var item in appointments)
                    //     {
                    //             if(item.StartTime.Date == dt.Date)
                    //             {
                    //                 filteredAppointments.Add(item);
                    //             }
                    //     }
                    // }
        //  return filteredAppointments.OrderBy(app => app.StartTime);
      }

      public Appointment GetAppointment(Guid id){
        return appointmentDL.GetAppointments().Where(appointment => appointment.Id == id).SingleOrDefault();
      }

      //Funtion to create appointment
      public string CreateAppointment(AppointmentDto appointmentDto){
                
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
 
                var appointments = appointmentDL.GetAppointments();
            
            // 409 response when there is a conflict
                foreach(var item in appointments)
                {
                    
                    if(item.StartTime.Date == appointment.EndTime.Date && item.EndTime.Date == appointment.EndTime.Date)
                    {
                        if(item.StartTime < appointment.StartTime && item.EndTime > appointment.StartTime)
                        {
                            var ErrorString = appointment.StartTime +" is conflicting with an existing appointment having startTime:" + item.StartTime+ " and endTime:" + item.EndTime;
                            return ErrorString;
                        }
                        if(appointment.EndTime>item.StartTime  && appointment.EndTime<item.StartTime)
                        {
                            var ErrorString = appointment.EndTime +" is conflicting with an existing appointment having startTime:" + item.StartTime+ " and endTime:" + item.EndTime;
                            return ErrorString;
                        }

                    }
                }

                appointmentDL.CreateAppointment(appointment);
                // add the appointment officially
                // appointments.Add(appointment);
                var stringId = appointment.Id.ToString();
                return stringId;
            
      }

        //funtion to delete an appointment
        public bool DeleteAppointment(Guid id)
        { 
            var appointments  = appointmentDL.GetAppointments().ToList();
            var index = appointments.FindIndex( existingItem => existingItem.Id == id);
            if(index == -1)
            {
                return false;
            }
            appointmentDL.DeleteAppointment(id);
            return true;
        }
    }
}