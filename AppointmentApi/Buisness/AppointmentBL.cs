using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AppointmentApi.DataAccess;
using AppointmentApi.Models;
namespace AppointmentApi.Buisness
{
   public class AppointmentBL: IAppointmentDL
   {
      private readonly List<Appointment> appointments = new()
      {
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 23, 00, 00), EndTime = new DateTime(2023, 10, 15, 23, 59, 00), Title= "go to Gym", Id = Guid.NewGuid()},
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 13, 00, 00), EndTime = new DateTime(2023, 10, 15, 13, 59, 00), Title= "go for a walk", Id = Guid.NewGuid()},
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 19, 00, 00), EndTime =new DateTime(2023, 10, 15, 19, 59, 00), Title= "go for cohee", Id = Guid.NewGuid()}   
      };

      public IEnumerable<Appointment> GetAppointments(){
           var SortedAppointments = appointments.OrderBy(appointment => appointment.StartTime);
           return SortedAppointments;
      }

      
      // This function fetches appointment by date
      public IEnumerable<Appointment> GetAppointmentsBydate(string date)
      {
                    // to check if the entered date format is correct or not
                    string regexPattern = @"^\d{2}-\d{2}-\d{4}$";
                    Regex regex = new Regex(regexPattern);
                    if(!regex.Match(date).Success || regexPattern == null)
                    {
                        return null;
                    }

                    // Filter appointments by date
                    List<Appointment> filteredAppointments = new List<Appointment>();
                    DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", null);
                    
                    foreach(var item in appointments)
                    {
                            if(item.StartTime.Date == dt.Date)
                            {
                                filteredAppointments.Add(item);
                            }
                    }
         return filteredAppointments.OrderBy(app => app.StartTime);
      }

      public Appointment GetAppointment(Guid id){
        return appointments.Where(appointment => appointment.Id == id).SingleOrDefault();
      }

      //Funtion to create appointment
      public string CreateAppointment(AppointmentDto appointmentDto){
                
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
                    return null;
                }

                // check for same date of appointment and if startTime == endTime
                if((appointment.StartTime == appointment.EndTime) || (appointment.StartTime.Date != appointment.EndTime.Date)){
                        return null;
                }

            
            // 409 response when there is a conflict
                    foreach(var item in appointments)
                    {
                        
                        if(item.StartTime.Date == appointment.EndTime.Date && item.EndTime.Date == appointment.EndTime.Date)
                        {
                            if((item.StartTime < appointment.StartTime && item.EndTime > appointment.StartTime) || (appointment.EndTime>item.StartTime  && appointment.EndTime<item.StartTime))
                            {
                                return "";
                            }

                        }
                    }

                  // add the appointment officially
                    appointments.Add(appointment);
                    var stringId = appointment.Id.ToString();
                    return stringId;
            
      }

        //funtion to delete an appointment
        public void DeleteAppointment(Guid id)
        {
            var index = appointments.FindIndex( existingItem => existingItem.Id == id);
            appointments.RemoveAt(index);
        }
    }
}