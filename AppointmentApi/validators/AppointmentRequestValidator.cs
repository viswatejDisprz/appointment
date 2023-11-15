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
             .WithMessage(ResponseErrors.NotEmpty());


             RuleFor(appointment => appointment.StartTime)
             .Must(NotBeOutDated)
             .WithMessage(ResponseErrors.GreaterThanCurrentTime("StartTime"));


             RuleFor(appointment => appointment.EndTime)
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
