using AppointmentApi.Buisness;
using Microsoft.AspNetCore.Mvc;
using AppointmentApi.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace AppointmentApi.Controllers
{

  // GET /appointments
  // [ApiController]
  [Route("v1/appointments")]
  public class AppointmentController : ControllerBase
  {

    private readonly IAppointmentBL _appointmentBL;

    public AppointmentController(IAppointmentBL appointmentBL)
    {
      _appointmentBL = appointmentBL;
    }


    /// <summary>
    /// Get Appointments for the day
    /// </summary>
    /// <remarks>
    /// Sample request Format:
    ///
    /// "Date": "{DynamicDateFormat}"
    ///
    /// </remarks>
    /// <returns>List of appointments</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Appointment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<CustomError>), StatusCodes.Status400BadRequest)]
    public List<Appointment> GetAppointments([FromQuery] AppointmentDateRequest appointmentDateRequest) =>
        _appointmentBL.GetAppointments(appointmentDateRequest);




    // Post / appointments
    /// <summary>
    /// Creates an appointment
    /// </summary>
    /// <param startTime="2023-10-12T09:09:09.619Z"> Start time in DateTime format to register appointment start time </param>
    /// <param  EndTime = "2023-10-12T09:09:09.619Z"> EndTime in DateTime format to register the endTime </param>
    /// <param  Title = "Appointment Title string"> Title of the appointment </param>
    /// <returns>The Id of the appointment created</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create an Appointment")]
    [ProducesResponseType(typeof(GuidValueResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<CustomError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomError), StatusCodes.Status409Conflict)]
    public ActionResult<GuidValueResult> CreateAppointment([FromBody] AppointmentRequest appointmentrequest) =>
       Created("", new GuidValueResult { Id = _appointmentBL.CreateAppointment(appointmentrequest) });



    // Delete / appointments
    // GET / appointments
    /// <summary>
    /// Delete the selected appointment
    /// </summary>
    /// <param Guid="3fa85f64-5717-4562-b3fc-2c963f66afa6">Guid to delete a particular appointment</param>
    /// <returns>No Content</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an Appointment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteAppointment(Guid id)
    {
      _appointmentBL.DeleteAppointment(id);
      return NoContent();
    }
  }
}