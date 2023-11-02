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

      DateOnly dateOnly = new DateOnly(appointmentrequest.StartTime.Year, appointmentrequest.StartTime.Month, appointmentrequest.StartTime.Day);
      var appointments = _appointmentDL.GetAppointments(null, dateOnly);

      var conflictingAppointment = appointments?.FirstOrDefault(item =>
       (item.StartTime < appointmentrequest.StartTime && item.EndTime > appointmentrequest.StartTime) ||
       (appointmentrequest.EndTime > item.StartTime && appointmentrequest.EndTime < item.StartTime));

      if (conflictingAppointment != null)
      {

        var errorString = (conflictingAppointment.StartTime < appointmentrequest.StartTime ?
                           appointmentrequest.StartTime : appointmentrequest.EndTime) +
                          "is conflicting with an existing appointment having startTime:" +
                         $"{conflictingAppointment.StartTime} and endTime: {conflictingAppointment.EndTime}";        
            throw new HttpResponseException(StatusCodes.Status409Conflict, new CustomError { Message = errorString });
      }

      return _appointmentDL.CreateAppointment(appointmentrequest);
    }
    public void DeleteAppointment(Guid id)
    {
      var result = _appointmentDL.GetAppointments(id, null);
      if (result.Count == 0)
      {
          throw new HttpResponseException(StatusCodes.Status404NotFound, new CustomError(){Message="Appointment not found"}); 
      }
      _appointmentDL.DeleteAppointment(id);
    }
  }
}