using FluentValidation;
using AppointmentApi.Models;

namespace AppointmentApi.validators
{
    public class AppointmentDateRequestValidator : AbstractValidator<AppointmentDateRequest>
    {
        public AppointmentDateRequestValidator()
        {
            RuleFor(apptDateRequest => apptDateRequest.Date)
                .NotEmpty()
                .WithMessage("Please enter the Correct date format in : MM/dd/yyyy");
        }

    }
}
