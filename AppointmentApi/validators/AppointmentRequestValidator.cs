using FluentValidation;
using AppointmentApi.Models;

namespace AppointmentApi.validators
{
  public class AppointmentRequestValidator : AbstractValidator<AppointmentRequest>
      {
         public AppointmentRequestValidator()
         {
            // validate appointment title for null string is null or whitespace check here
             // write extension method for validation failures that method will handle it
             RuleFor(appointment => appointment.Title)
             .NotEmpty()
             .WithMessage("Appointment Title should not be empty");

             // validate startTime for null and firmat
             RuleFor(appointment => appointment.StartTime)
             .NotEmpty()
             .Must(BeAValidDate)
             .WithMessage("Start Time must be a valid date and time.");

             // validate EndTime for null and firmat
             RuleFor(appointment => appointment.EndTime)
             .NotEmpty()
             .Must(BeAValidDate)
             .WithMessage("End Time must be a valid date and time.")
             .Must((appointmentRequest, endTime) => endTime > appointmentRequest.StartTime)
             .WithMessage("End time must be greater than Start Time.")
             .Must((appointmentRequest, endTime) => endTime.Date == appointmentRequest.StartTime.Date)
             .WithMessage("Appointment can only be set for same day endTime and StartTime should have same date");

         }

         // function to check format of time and also keep it in bounds
         private bool BeAValidDate(DateTime date)
          {
             return (date != DateTime.MinValue) || (date != DateTime.MaxValue);
          }
      }
}
