using System.Net;
using AppointmentApi.DataAccess;
using AppointmentApi.Models;
using AppointmentApi.validators;
using Microsoft.AspNetCore.Http.HttpResults;

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
      var test=appointmentDateRequest.Date.ToString("MM/DD/YYYY");
      Console.WriteLine(test);
      //  AppointmentDateRequestValidator appointmentDateRequestValidator = new AppointmentDateRequestValidator();
      //  FluentValidation.Results.ValidationResult results = appointmentDateRequestValidator.Validate(appointmentDateRequest);
      //  if(!results.IsValid)
      //  {

          // var firstError = results.Errors.FirstOrDefault();
          // Console.WriteLine(firstError.ErrorMessage);
          // CustomError error = new CustomError(){Message = firstError.ErrorMessage};
          // throw new HttpResponseException(error);
          // return error.CustomException(HttpStatusCode.BadRequest);
          // throw new HttpResponseException(StatusCodes.Status400BadRequest, error);
          appointmentDateRequest.Validate<AppointmentDateRequest, AppointmentDateRequestValidator>();
      //  }
       return _appointmentDL.GetAppointments(date: appointmentDateRequest.Date);

    }
    public Guid CreateAppointment(AppointmentRequest appointmentrequest)
    {

      return _appointmentDL.CreateAppointment(appointmentrequest);

    }
    public void DeleteAppointment(Guid id)
    {

      _appointmentDL.DeleteAppointment(id);
    }
  }
}