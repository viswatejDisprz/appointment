using AppointmentApi.DataAccess;
using AppointmentApi.Models;
namespace AppointmentApi.Buisness
{
   public class AppointmentDL: IAppointmentDL
   {
      private List<Appointment> appointments = new()
      {
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 23, 00, 00), EndTime = new DateTime(2023, 10, 15, 23, 59, 00), Title= "go to Gym", Id = Guid.NewGuid()},
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 13, 00, 00), EndTime = new DateTime(2023, 10, 15, 13, 59, 00), Title= "go for a walk", Id = Guid.NewGuid()},
        new Appointment{ StartTime = new DateTime(2023, 10, 15, 19, 00, 00), EndTime =new DateTime(2023, 10, 15, 19, 59, 00), Title= "go for cohee", Id = Guid.NewGuid()}   
      };

      
      // This function fetches appointment by date
      public List<Appointment> GetAppointments(Guid? id = null,DateOnly? date = null) // change this to data layer
      {

                    // Filter appointments by date
                 if(date is not null)
                 {
                    TimeOnly timeOnly = new TimeOnly(12, 30, 0);
                    DateOnly dateOnly = date.Value;

                    var filteredAppointments = appointments.Where(app => app.StartTime.Date == dateOnly.ToDateTime(timeOnly).Date);
                    return filteredAppointments.ToList();
                 }
                 else if ( id is not null)
                 {
                     var filteredAppointments = appointments.Where(app => app.Id == id);
                     return filteredAppointments.ToList();
                 }else
                 {
                     return appointments;
                 }
      }

      //Funtion to create appointment
      public Guid CreateAppointment(Appointment appointment){ //changes return guid
                // add the appointment officially
                appointments.Add(appointment);
                return appointment.Id;
            
      }

        //funtion to delete an appointment
        public void DeleteAppointment(Guid id)
        {
            var index = appointments.FindIndex( existingItem => existingItem.Id == id);
            appointments.RemoveAt(index);
        }
    }
}