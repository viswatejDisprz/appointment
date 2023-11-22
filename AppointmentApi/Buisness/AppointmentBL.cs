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

      ValidateAppointmentConflicts(appointmentrequest);

      return _appointmentDL.CreateAppointment(appointmentrequest);
    }

    public void DeleteAppointment(Guid id)
    {
      var appointment = _appointmentDL.GetAppointment(id);
      if (appointment is null)
        throw new HttpResponseException(StatusCodes.Status404NotFound);

      _appointmentDL.DeleteAppointment(id);
    }

    // Appointment Conflict checking function
    private void ValidateAppointmentConflicts(AppointmentRequest appointmentrequest)
    {

      var appointments = _appointmentDL.GetAppointments(DateOnly.FromDateTime(appointmentrequest.StartTime));

      var conflictingAppointment = appointments.FirstOrDefault(item =>
        (appointmentrequest.StartTime >= item.StartTime && appointmentrequest.StartTime <= item.EndTime) ||
        (appointmentrequest.EndTime >= item.StartTime && appointmentrequest.EndTime <= item.EndTime) ||
        (appointmentrequest.StartTime <= item.StartTime && appointmentrequest.EndTime >= item.EndTime)
      );

      if (conflictingAppointment != null)
      {
        var ConflictTime = (conflictingAppointment.StartTime >= appointmentrequest.StartTime && conflictingAppointment.EndTime <= appointmentrequest.EndTime) ||
                           (conflictingAppointment.StartTime < appointmentrequest.StartTime && conflictingAppointment.EndTime > appointmentrequest.EndTime)
                            ?
                          "StartTime and EndTime" :
                          (conflictingAppointment.StartTime < appointmentrequest.StartTime ?
                          "StartTime" : "EndTime");

        throw ResponseErrors.ConflictError(ConflictTime, conflictingAppointment.StartTime, conflictingAppointment.EndTime)
              .CustomException(StatusCodes.Status409Conflict);
      }
    }
  }
}