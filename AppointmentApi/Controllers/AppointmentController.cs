using System.Text.RegularExpressions;
using AppointmentApi.Buisness;
using AppointmentApi.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AppointmentApi.Models;

namespace AppointmentApi.Controllers
{
    // GET /appointments
    [ApiController]
    [Route("v1/appointments")]
    public class AppointmentController : ControllerBase
    {
       private readonly IAppointmentDL repository;

       private readonly ILogger<AppointmentController> _logger;

       public AppointmentController(ILogger<AppointmentController> logger,IAppointmentDL repository){
         this.repository = repository;
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
       public IActionResult GetAppointments(string date)
       {
                try{

                    var filteredAppointments = repository.GetAppointmentsBydate(date);
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
                   var response = repository.CreateAppointment(appointmentDto);
                   if(response == null)
                   {
                      CustomError error = new CustomError() { Message = "Bad Request"};
                      return BadRequest(error); 
                   }
                   else if(response == "")
                   {
                       CustomError error = new CustomError() { Message = "Conflict Error"};
                       return  StatusCode(409, error);;
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
                    var appointment = repository.GetAppointment(id);
                    if( appointment is null){
                        return NotFound();
                    }
                    repository.DeleteAppointment(id);
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