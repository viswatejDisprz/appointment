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
        new Appointment{ StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow, Title= "go to Gym", Id = Guid.NewGuid()},
        new Appointment{ StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow, Title= "go for a walk", Id = Guid.NewGuid()},
        new Appointment{ StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow, Title= "go for cohee", Id = Guid.NewGuid()}   
      };

      public IEnumerable<Appointment> GetAppointments(){
           return appointments;
      }

      public IEnumerable<Appointment> GetAppointmentsBydate(string date)
      {
                    string regexPattern = @"^\d{2}-\d{2}-\d{4}$";
                    Regex regex = new Regex(regexPattern);
                    if(!regex.Match(date).Success)
                    {
                        ErrorDto BadReq  = new ErrorDto {Message = "Bad Request"};
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
         return appointments;
      }

      public Appointment GetAppointment(Guid id){
        return appointments.Where(appointment => appointment.Id == id).SingleOrDefault();
      }

      public string CreateAppointment(AppointmentDto appointmentDto){

                if(appointmentDto.IsValid() || (appointmentDto.StartTime == appointmentDto.EndTime)){
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