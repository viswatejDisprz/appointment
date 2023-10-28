using FluentValidation;
using AppointmentApi.Models;

namespace AppointmentApi.validators
{
    public class AppointmentDateRequestValidator : AbstractValidator<AppointmentDateRequest>
    {
        /// DateOnly does not support ToString() conversion in C#
        // public AppointmentDateRequestValidator()
        // {
        //     RuleFor(apptDateRequest => apptDateRequest.Date.ToString("MM/dd/yyyy"))
        //         .NotEmpty().WithMessage("Date format should not be empty")
        //         .Matches(@"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/[0-9]{4}$")
        //         .WithMessage("Date format should be in MM/DD/YYYY");
        // }
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
