using FluentValidation;
using AppointmentApi.Models;

namespace AppointmentApi.validators
{
    public class AppointmentDateRequestValidator : AbstractValidator<AppointmentDateRequest>
    {
        public AppointmentDateRequestValidator()
        {
            RuleFor(apptDateRequest => apptDateRequest.Date)
                .NotEmpty().WithMessage("Date format should be in MM/DD/YYYY")
                .Must(BeAValidDate).WithMessage("Date format should be in MM/DD/YYYY");
        }
        private bool BeAValidDate(DateOnly date)
        {
            return DateTime.TryParseExact(date.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}
