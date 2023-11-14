using FluentValidation;
using AppointmentApi.Models;
using Azure;

namespace AppointmentApi.validators
{
  public class AppointmentRequestValidator : AbstractValidator<AppointmentRequest>
      {
         public AppointmentRequestValidator()
         {
             RuleFor(appointment => appointment.Title)
             .NotEmpty()
             .WithMessage(ResponseErrors.NotEmpty("Title"));


             RuleFor(appointment => appointment.StartTime)
             .NotNull()
             .NotEmpty().WithMessage(ResponseErrors.NotEmpty("StartTime"))
             .Must(NotBeOutDated)
             .WithMessage(ResponseErrors.GreaterThanCurrentTime("StartTime"));


             RuleFor(appointment => appointment.EndTime)
             .NotEmpty().WithMessage(ResponseErrors.NotEmpty("EndTime"))
             .Must((appointmentRequest, endTime) => endTime > appointmentRequest.StartTime)
             .WithMessage(ResponseErrors.EndtimeGreaterThanStartTime())
             .Must((appointmentRequest, endTime) => endTime.Date == appointmentRequest.StartTime.Date)
             .WithMessage(ResponseErrors.SameDay())
             .Must(NotBeOutDated)
             .WithMessage(ResponseErrors.GreaterThanCurrentTime("EndTime"));

         }

        private bool NotBeOutDated(DateTime appointmentTime)
         {
             var currentDateTime = DateTime.Now;
             if(appointmentTime < currentDateTime)
             {
                return  false;
             }
             return true;
         }
      }
}
