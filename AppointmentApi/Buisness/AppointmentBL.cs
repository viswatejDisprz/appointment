using AppointmentApi.DataAccess;
using AppointmentApi.Models;
using AppointmentApi.validators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Razor.Language;

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
       (item.StartTime < appointmentrequest.EndTime && item.EndTime > appointmentrequest.EndTime) ||
       (appointmentrequest.StartTime < item.EndTime && appointmentrequest.StartTime > item.StartTime) ||
       (item.StartTime > appointmentrequest.StartTime && item.EndTime < appointmentrequest.EndTime));

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
          throw ResponseErrors.NotFound.CustomException(StatusCodes.Status404NotFound); 
      }
      _appointmentDL.DeleteAppointment(id);
    }
  }
}