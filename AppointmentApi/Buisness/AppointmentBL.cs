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
       var appointments = _appointmentDL.GetAppointments(null,dateOnly);

       var conflictingAppointment = appointments.FirstOrDefault(item =>
        (item.StartTime < appointmentrequest.StartTime && item.EndTime > appointmentrequest.StartTime) ||
        (appointmentrequest.EndTime > item.StartTime && appointmentrequest.EndTime < item.StartTime));

        if (conflictingAppointment != null)
        {
            var errorString = conflictingAppointment.StartTime < appointmentrequest.StartTime ?
                appointmentrequest.StartTime + " is conflicting with an existing appointment having startTime: " +
                conflictingAppointment.StartTime + " and endTime: " + conflictingAppointment.EndTime :
                appointmentrequest.EndTime + " is conflicting with an existing appointment having startTime: " +
                conflictingAppointment.StartTime + " and endTime: " + conflictingAppointment.EndTime;

            var error = new CustomError { Message = errorString };
            throw new HttpResponseException(409, error);
        }

       return _appointmentDL.CreateAppointment(appointmentrequest);
    }
    public void DeleteAppointment(Guid id)
    {
      if(_appointmentDL.GetAppointments(id,null).Count == 0)
      {
        var error = new CustomError { Message = "Appointment not found" };
        throw new HttpResponseException(404, error);
      }
      _appointmentDL.DeleteAppointment(id);
    }
  }
}