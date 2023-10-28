// using FluentValidation;
// using AppointmentApi.Models;
// using System;

// namespace AppointmentApi.validators
// {
//     public class ApptDateValidator: AbstractValidator<AppointmentDateRequest>
//     {
//         public ApptDateValidator()
//         {
//             RuleFor(apptDateRequest => apptDateRequest.Date)
//                 .NotEmpty().WithMessage("Date format should be in MM-DD-YYYY")
//                 .Must(BeAValidDate).WithMessage("Date format should be in MM-DD-YYYY");
//         }

//         private bool BeAValidDate(DateOnly date)
//         {
//             if (DateTime.TryParse(date.ToString(), out DateTime result))
//             {
//                 bool flag = System.Text.RegularExpressions.Regex.IsMatch(date.ToString(), @"^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$");
//                 return flag;
//             }
//             return false;
//         }
//     }
// }

using FluentValidation;
using AppointmentApi.Models;
using System;

namespace AppointmentApi.validators
{
    public class AppointmentDateRequestValidator : AbstractValidator<AppointmentDateRequest>
    {
        public AppointmentDateRequestValidator()
        {
            RuleFor(apptDateRequest => apptDateRequest.Date.ToString("MM/DD/YYYY"))
                .NotEmpty().WithMessage("Date format should be in 10/15/2023")
                .Matches(@"^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$")
                .WithMessage("Date format should be in MM/DD/YYYY");
        }
    }
}
