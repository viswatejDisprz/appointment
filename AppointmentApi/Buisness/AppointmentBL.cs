using AppointmentApi.DataAccess;
using AppointmentApi.Models;
using AppointmentApi.validators;

namespace AppointmentApi.Buisness
{
  public class AppointmentBL : IAppointmentBL
  {
    private readonly IAppointmentDL _appointmentDL;

    public AppointmentBL(IAppointmentDL appointmentDL)
    {
      _appointmentDL = appointmentDL;
    }
    public List<Appointment> GetAppointments(AppointmentDateRequest appointmentDateRequest)
    {
      appointmentDateRequest.Validate<AppointmentDateRequest, AppointmentDateRequestValidator>();
      return _appointmentDL.GetAppointments(date: appointmentDateRequest.Date);

    }
    public Guid CreateAppointment(AppointmentRequest appointmentrequest)
    {
      appointmentrequest.Validate<AppointmentRequest, AppointmentRequestValidator>();

      var appointments = _appointmentDL.GetAppointments(null, DateOnly.FromDateTime(appointmentrequest.StartTime));

      var conflictingAppointment = FilterConflictingAppointments(appointments,appointmentrequest);

      if (conflictingAppointment != null)
      {
        var ConflictTime = conflictingAppointment.StartTime < appointmentrequest.StartTime ?
                        appointmentrequest.StartTime : appointmentrequest.EndTime;
                        
        throw ResponseErrors.ConflictError(ConflictTime, conflictingAppointment.StartTime, conflictingAppointment.EndTime)
              .CustomException(StatusCodes.Status409Conflict);
      }

      return _appointmentDL.CreateAppointment(appointmentrequest);
    }
    public void DeleteAppointment(Guid id)
    {
      var result = _appointmentDL.GetAppointments(id, null);
      if (!result.Any())
      {
        throw Extensions.CustomException(StatusCodes.Status404NotFound); 
      }
      _appointmentDL.DeleteAppointment(id);
    }


   private Appointment? FilterConflictingAppointments(List<Appointment> appointments, AppointmentRequest appointmentrequest)
   {
      return appointments?.FirstOrDefault(item =>
       (appointmentrequest.StartTime <= item.StartTime && appointmentrequest.EndTime >= item.EndTime) ||
       (appointmentrequest.EndTime >= item.StartTime && appointmentrequest.EndTime <= item.EndTime) ||
       (appointmentrequest.StartTime <= item.EndTime && appointmentrequest.StartTime >= item.StartTime));
   }
  }
}