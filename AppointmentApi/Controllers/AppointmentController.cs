using AppointmentApi.Buisness;
using Microsoft.AspNetCore.Mvc;
using AppointmentApi.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace AppointmentApi.Controllers
{
    // GET /appointments
    [ApiController]
    [Route("v1/appointments")]
    public class AppointmentController : ControllerBase
    {
     
       private readonly IAppointmentBL appointmentBL;
       private readonly ILogger<AppointmentController> _logger;

       public AppointmentController(ILogger<AppointmentController> logger,IAppointmentBL appointmentBL){
         this.appointmentBL = appointmentBL;
         _logger = logger;
       }

       // GET / appointments
       /// <summary>
       /// Gets the list of appointments.
       /// </summary>
       /// <param date="12-12-2023">Date string to fetch appointments for a particular day</param>
       /// <returns>List of appointments as List</returns>
       [HttpGet]
       [ProducesResponseType(typeof(List<Appointment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<CustomError>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomError), StatusCodes.Status500InternalServerError)]
       public ActionResult<List<Appointment>> GetAppointments(DateOnly date)
        {
            /// refer the file in chat to respond from buisnees layer itself
            try{

                var filteredAppointments = appointmentBL.GetAppointments(null,date);
                if(filteredAppointments == null)
                {
                    CustomError error  = new CustomError(){Message = "Please Enter correct format of Date DD-MM-YYYY"};
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
       /// Just calling
       [HttpPost]
       [SwaggerOperation(Summary = "Create an Appointment")]
       public IActionResult CreateAppointment(AppointmentRequest appointmentrequest)
       {
            try{
                   var response = appointmentBL.CreateAppointment(appointmentrequest);
                   var ConflictString = "conflicting";
                   if(response == "Input Invalid")
                   {
                      CustomError Starterror = new CustomError() { Message = "StartTime should be in the folowing format \"2023-10-17T06:04:02.475Z\""};
                      CustomError Enderror = new CustomError() { Message = "EndTime should be in the folowing format \"2023-10-17T06:04:02.475Z\""};
                      CustomError titleError = new CustomError() { Message = "Title should not be empty"};
                      List<CustomError> errArray = new()
                      {
                        Starterror,
                        Enderror,
                        titleError
                      };
                      return BadRequest(errArray); 
                   }
                   else if (response == "End time must be greater than Start Time.")
                   {
                      CustomError error = new CustomError() { Message = "End time cannot be less than or equal to Start Time"};
                       return  BadRequest(error);
                   }
                   else if(response.IndexOf(ConflictString, StringComparison.CurrentCultureIgnoreCase) != -1)
                   {
                       CustomError error = new CustomError() { Message = response};
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
                    var flag = appointmentBL.DeleteAppointment(id);
                    if(!flag){
                        CustomError error = new(){Message = "Appointment not found"};
                        return NotFound(error);
                    }else{
                       return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    // Return a custom 500 response
                    return StatusCode(500, "Internal Server Error: " + ex.Message);
                }
       }
    }
}