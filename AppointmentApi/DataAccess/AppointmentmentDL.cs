using AppointmentApi.DataAccess;
using AppointmentApi.Models;
namespace AppointmentApi.Buisness
{
  public class AppointmentDL : IAppointmentDL
  {
    private List<Appointment> appointments = new();
    
    public List<Appointment> GetAppointments(DateOnly date)
      => appointments.Where(app => DateOnly.FromDateTime(app.StartTime) == date).ToList();
     
    public Appointment GetAppointment(Guid id) => appointments.Find(item => item.Id == id);

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