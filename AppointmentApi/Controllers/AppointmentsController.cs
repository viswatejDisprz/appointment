// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using AppointmentApi.Models;
// using Microsoft.Extensions.Logging;

// namespace AppointmentApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AppointmentsController : ControllerBase
//     {
//         // private readonly ILogger<AppointmentsController> _logger;

//         // public AppointmentsController(ILogger<AppointmentsController> logger)
//         // {
//         //     _logger = logger;
//         // }
//         // private readonly AppointmentContext _context;

//         // public AppointmentsController(AppointmentContext context)
//         // {
//         //     _context = context;
//         // }

//         private readonly ILogger<AppointmentsController> _logger;
//         private readonly AppointmentContext _context;

//         public AppointmentsController(ILogger<AppointmentsController> logger, AppointmentContext context)
//         {
//             _logger = logger;
//             _context = context;
//         }

//         // GET: api/Appointments
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment()
//         {
//           if (_context.Appointment == null)
//           {
//               return NotFound();
//           }
//             return await _context.Appointment.ToListAsync();
//         }

//         // GET: api/Appointments/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
//         {
//           if (_context.Appointment == null)
//           {
//               return NotFound();
//           }
//             var appointment = await _context.Appointment.FindAsync(id);

//             if (appointment == null)
//             {
//                 return NotFound();
//             }

//             return appointment;
//         }

//         // PUT: api/Appointments/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutAppointment(Guid id, Appointment appointment)
//         {
//             if (id != appointment.Id)
//             {
//                 return BadRequest();
//             }

//             _context.Entry(appointment).State = EntityState.Modified;

//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!AppointmentExists(id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }

//             return NoContent();
//         }

//         // POST: api/Appointments
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
//         {
//           if (_context.Appointment == null)
//           {
//               return Problem("Entity set 'AppointmentContext.Appointment'  is null.");
//           }
//             _logger.LogInformation($"afhadsbjlf aksfalsk glskd {appointment.Title}");
//             _context.Appointment.Add(appointment);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
//         }

//         // DELETE: api/Appointments/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteAppointment(Guid id)
//         {
//             if (_context.Appointment == null)
//             {
//                 return NotFound();
//             }
//             var appointment = await _context.Appointment.FindAsync(id);
//             if (appointment == null)
//             {
//                 return NotFound();
//             }

//             _context.Appointment.Remove(appointment);
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }

//         private bool AppointmentExists(Guid id)
//         {
//             return (_context.Appointment?.Any(e => e.Id == id)).GetValueOrDefault();
//         }
//     }
// }


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
           var k = (List<Appointment>)repository.GetAppointments();
        
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
           IdDto id  = new IdDto {Id = appointment.Id, k1 = (List<Appointment>)repository.GetAppointments()};
           return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, id);

       }
    }
}