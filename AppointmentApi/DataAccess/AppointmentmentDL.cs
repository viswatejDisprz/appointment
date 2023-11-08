using AppointmentApi.DataAccess;
using AppointmentApi.Models;
namespace AppointmentApi.Buisness
{
  public class AppointmentDL : IAppointmentDL
  {
    private List<Appointment> appointments = new();
    
    public List<Appointment> GetAppointments(Guid? id = null, DateOnly? date = null)
    {
      // var condition = new Func<Appointment, bool>(app => date.HasValue ? DateOnly.FromDateTime(app.StartTime) == date : app.Id == id);
      // return appointments.Where(condition).ToList();
      if(date.HasValue) 
         return appointments.Where(app => DateOnly.FromDateTime(app.StartTime) == date).ToList();
      
      // else if(id.HasValue)
      return appointments.Where(app => app.Id == id).ToList();

      // else
      //    return appointments.ToList();
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

    public void DeleteAppointment(Guid id) =>
      appointments.RemoveAt(appointments.FindIndex(existingItem => existingItem.Id == id));

  }
}