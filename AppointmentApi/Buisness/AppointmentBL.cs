using System;
using System.Collections.Generic;
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

      public IEnumerable<Appointment> GetAppointmentsBydate(string date)
      {
                    string regexPattern = @"^\d{2}-\d{2}-\d{4}$";
                    Regex regex = new Regex(regexPattern);
                    if(!regex.Match(date).Success || regexPattern == null)
                    {
                        return [];
                    }

                    List<Appointment> filteredAppointments = new();
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

      public string CreateAppointment(AppointmentDto appointmentDto){
                
                // check for Dto validity, same date of appointment and startTime!=endTime
                if(appointmentDto.IsValid() || (appointmentDto.StartTime == appointmentDto.EndTime) || (appointmentDto.StartTime.Date != appointmentDto.EndTime.Date)){
                        return null;
                }
                
                var appointment = new Appointment
                {
                    Title = appointmentDto.Title,
                    StartTime = appointmentDto.StartTime,
                    EndTime = appointmentDto.EndTime,
                    Id = Guid.NewGuid()
                };

            
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

                    appointments.Add(appointment);
                    var stringId = appointment.Id.ToString();
                    return stringId;
            
      }

        public void DeleteAppointment(Guid id)
        {
            var index = appointments.FindIndex( existingItem => existingItem.Id == id);
            appointments.RemoveAt(index);
        }
    }
}