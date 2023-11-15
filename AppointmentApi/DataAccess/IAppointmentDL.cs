using AppointmentApi.Models;
namespace AppointmentApi.DataAccess
{
  public interface IAppointmentDL
  {
    // Used to fetch appointments
    // GetAppointments takes id or date as input
    // returns list of appointments 
    List<Appointment> GetAppointments(DateOnly date);

    Appointment GetAppointment(Guid id);

    // Used to Add a new appointment to the list
    //CreateAppointment takes AppointmentRequest Dto as input
    // returns the guid of the appointment i.e newly created
    Guid CreateAppointment(AppointmentRequest appointment);

    // Used to Delete an appointment from the list
    // DeleteApointment takes guid  as an argument
    // returns nothing
    void DeleteAppointment(Guid id);
  }
}