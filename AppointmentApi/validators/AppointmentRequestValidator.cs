using FluentValidation;
using AppointmentApi.Models;

namespace AppointmentApi.validators
{
  public class AppointmentRequestValidator : AbstractValidator<AppointmentRequest>
      {
         public AppointmentRequestValidator()
         {
             RuleFor(appointment => appointment.Title)
             .NotEmpty()
             .WithMessage("Appointment Title should not be empty");


             RuleFor(appointment => appointment.StartTime)
             .NotNull()
             .NotEmpty().WithMessage("Appointment StartTime should not be empty");


             RuleFor(appointment => appointment.EndTime)
             .NotEmpty().WithMessage("Appointment EndTime should not be empty")
             .Must((appointmentRequest, endTime) => endTime > appointmentRequest.StartTime)
             .WithMessage("End time must be greater than Start Time.")
             .Must((appointmentRequest, endTime) => endTime.Date == appointmentRequest.StartTime.Date)
             .WithMessage("Appointment can only be set for same day endTime and StartTime should have same date");

         }
      }
}
