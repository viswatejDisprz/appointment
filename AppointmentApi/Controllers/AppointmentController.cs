using System.Text.RegularExpressions;
using AppointmentApi.Buisness;
using AppointmentApi.DataAccess;
using Microsoft.AspNetCore.Mvc;
using AppointmentApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Models;


namespace AppointmentApi.Controllers
{
    // GET /appointments
    [ApiController]
    [Route("v1/appointments")]
    public class AppointmentController : ControllerBase
    {
       private readonly IAppointmentDL repository;
     
       private readonly IAppointmentBL appointmentBL;
       private readonly ILogger<AppointmentController> _logger;

       public AppointmentController(ILogger<AppointmentController> logger,IAppointmentBL repository){
         this.appointmentBL = repository;
         _logger = logger;
       }

       // GET / appointments
       /// <summary>
       /// Gets the list of appointments.
       /// </summary>
       /// <param date="12-12-2023">Date string to fetch appointments for a particular day</param>
       /// <returns>List of appointments as List</returns>
       [HttpGet]
       [SwaggerOperation(Summary = "Get appointments for a particular date")]
       public ActionResult<List<Appointment>> GetAppointments(DateOnly date)
        {
            try{

                var filteredAppointments = appointmentBL.GetAppointmentsBydate(date);
                if(filteredAppointments == null)
                {
                    CustomError error  = new CustomError(){Message = "Bad Request"};
                    return BadRequest(error);
                }
                return Ok(filteredAppointments);
            }
            catch (Exception ex)
            {
                // Return a custom 500 response
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

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
       public IActionResult CreateAppointment(AppointmentDto appointmentDto)
       {
            try{
                   Appointment appointment = new Appointment
                    {
                        StartTime = appointmentDto.StartTime,
                        EndTime = appointmentDto.EndTime,
                        Title = appointmentDto.Title,
                        Id = Guid.NewGuid()
                    };
                   var response = appointmentBL.CreateAppointment(appointment);
                   if(response == "Input Invalid")
                   {
                      CustomError error = new CustomError() { Message = "Input Invalid"};
                      return BadRequest(error); 
                   }
                   else if (response == "End time less than Start Time")
                   {
                      CustomError error = new CustomError() { Message = "End time less than Start Time"};
                       return  StatusCode(409, error);
                   }
                   else if(response == "")
                   {
                       CustomError error = new CustomError() { Message = "Conflict Error"};
                       return  StatusCode(409, error);
                   }else{
                      // 201 response for post 
                      Guid Id1 = new Guid(response);
                      GuidValueResult id  = new GuidValueResult {Id = Id1};
                      return CreatedAtAction(nameof(GetAppointments), new { id = Id1 }, id);
                   }
                }
                catch (Exception ex)
                {
  
                    // Return a custom 500 response
                    return StatusCode(500, "Internal Server Error: " + ex.Message);
                }

       }


       // Delete / appointments
       // GET / appointments
       /// <summary>
       /// Delete the selected appointment
       /// </summary>
       /// <param Guid="3fa85f64-5717-4562-b3fc-2c963f66afa6">Guid to delete a particular appointment</param>
       /// <returns>No Content</returns>
       [HttpDelete("{id}")]
       [SwaggerOperation(Summary = "Delete an Appointment")]
       public ActionResult DeleteAppointment(Guid id)
       {
                try{
                    var appointment = appointmentBL.GetAppointment(id);
                    if( appointment is null){
                        return NotFound();
                    }
                    appointmentBL.DeleteAppointment(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    // Return a custom 500 response
                    return StatusCode(500, "Internal Server Error: " + ex.Message);
                }
       }
    }
}