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
                .WithMessage(ValidDateFormat);
        }

        private string ValidDateFormat(AppointmentDateRequest request)
        {
            var Dateformat = Extensions.GetDynamicDateFormat();
            return $"Please enter the Correct date format in : {Dateformat}";
        }
    }
}
