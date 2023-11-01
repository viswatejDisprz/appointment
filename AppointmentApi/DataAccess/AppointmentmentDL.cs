using AppointmentApi.DataAccess;
using AppointmentApi.Models;
namespace AppointmentApi.Buisness
{
  public class AppointmentDL : IAppointmentDL
  {
    private List<Appointment> appointments = new(){
      new Appointment {
        StartTime = new DateTime(2023, 10, 15, 23, 00, 00), EndTime = new DateTime(2023, 10, 15, 23, 59, 00), Title = "go to Gym", Id = Guid.NewGuid()
      },
      new Appointment {
        StartTime = new DateTime(2023, 10, 15, 13, 00, 00), EndTime = new DateTime(2023, 10, 15, 13, 59, 00), Title = "go for a walk", Id = Guid.NewGuid()
      },
      new Appointment {
        StartTime = new DateTime(2023, 10, 15, 19, 00, 00), EndTime = new DateTime(2023, 10, 15, 19, 59, 00), Title = "go for cohee", Id = Guid.NewGuid()
      }
    };
    public List<Appointment> GetAppointments(Guid? id = null, DateOnly? date = null)
    {
      var condition = new Func<Appointment, bool>(app => date.HasValue ? DateOnly.FromDateTime(app.StartTime) == date : app.Id == id);
      return appointments.Where(condition).ToList();
    }

    public Guid CreateAppointment(AppointmentRequest appointment)
    {
      var id = Guid.NewGuid();

      appointments.Add(new Appointment
      {
        Title = appointment.Title,
        StartTime = appointment.StartTime,
        EndTime = appointment.EndTime,
        Id = id
      });

      return id;

    }
    public void DeleteAppointment(Guid id)
    {
      var index = appointments.FindIndex(existingItem => existingItem.Id == id);
      // if(index == -1)
      // {
      //   return;
      // }
      appointments.RemoveAt(appointments.FindIndex(existingItem => existingItem.Id == id));
    }

  }
}