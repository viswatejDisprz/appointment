using AppointmentApi.Models;
namespace AppointmentApi.DataAccess
{
  public interface IAppointmentDL
  {
    List<Appointment> GetAppointments(Guid? id = null, DateOnly? date = null);

    Guid CreateAppointment(AppointmentRequest appointment);

    void DeleteAppointment(Guid id);
  }
}