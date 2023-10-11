using AppointmentApi.Dtos;
using AppointmentApi.Models;
using AppointmentApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace AppointmentApi.Controllers
{
    // GET /appointments
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {
       private readonly IAppointmentRepository repository;
       private readonly ILogger<AppointmentsController> _logger;

       public AppointmentsController(ILogger<AppointmentsController> logger,IAppointmentRepository repository){
         this.repository = repository;
         _logger = logger;
       }



       // GET / appointments
       [HttpGet]
       public IEnumerable<Appointment> GetAppointments()
       {
           var appointments = repository.GetAppointments();
           return appointments;
       }

       // Get / appointments/{id}
       [HttpGet("{id}")]
       public ActionResult<AppointmentDto> GetAppointment(Guid id)
       {
            try{var appointment = repository.GetAppointment(id);
            if(appointment is null){
                return NotFound();
            }
            return appointment.AsDto();}
            catch (Exception ex)
            {
            
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
       }


       // Post / appointments
       [HttpPost]
       public ActionResult<IPostDto> CreateAppointment(AppointmentDto appointmentDto)
       {

           
           var appointment = new Appointment
           {
               Title = appointmentDto.Title,
               StartTime = appointmentDto.StartTime,
               EndTime = appointmentDto.EndTime,
               Id = Guid.NewGuid()
           };
           
           if(appointmentDto.IsValid()){
            return StatusCode(500, "Internal server error");
           }

        
         // 409 response when there is a conflict
           foreach(var item in repository.GetAppointments())
           {
             
              if(item.StartTime.Date == appointment.EndTime.Date && item.EndTime.Date == appointment.EndTime.Date)
              {
                if((item.StartTime < appointment.StartTime && item.EndTime > appointment.StartTime) || (appointment.EndTime>item.StartTime  && appointment.EndTime<item.StartTime))
                {
                    return StatusCode(409, appointment.AsDto());
                }

              }
           }

           repository.CreateAppointment(appointment);
           
           // 201 response for post 
           IdDto id  = new IdDto {Id = appointment.Id};
           return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, id);

       }
    }
}