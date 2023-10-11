using System;
using System.Collections.Generic;
using AppointmentApi.Models;

namespace AppointmentApi.Repositories
{
   public class InMemAppointmentRepository: IAppointmentRepository
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

      public Appointment GetAppointment(Guid id){
        return appointments.Where(appointment => appointment.Id == id).SingleOrDefault();
      }

      public void CreateAppointment(Appointment appointment){
        appointments.Add(appointment);
      }
   }
}