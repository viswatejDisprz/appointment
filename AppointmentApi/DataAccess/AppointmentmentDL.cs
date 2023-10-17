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

      public List<Appointment> GetAppointments(){
           var SortedAppointments = appointments.OrderBy(appointment => appointment.StartTime);
           return SortedAppointments.ToList();
      }

      
      // This function fetches appointment by date
      public IEnumerable<Appointment> GetAppointmentsBydate(DateOnly date) // change this to data layer
      {

                    // Filter appointments by date
                    List<Appointment> filteredAppointments = new List<Appointment>();
                    TimeOnly timeOnly = new TimeOnly(12, 30, 0);
                    DateTime dt = date.ToDateTime(timeOnly);

                    foreach(var item in appointments) // use LINQ's never foreach
                    {
                            if(item.StartTime.Date == dt.Date)
                            {
                                filteredAppointments.Add(item);
                            }
                    }
         return filteredAppointments.OrderBy(app => app.StartTime);
      }

      public Appointment GetAppointment(Guid id){ // Notrequired
        return appointments.Where(appointment => appointment.Id == id).SingleOrDefault();
      }

      //Funtion to create appointment
      public string CreateAppointment(Appointment appointment){ //changes return guid
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