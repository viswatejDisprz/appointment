// An extension is defined to conevert appointmentDto to appointment which includes Id
using AppointmentApi.Models;
using FluentValidation;
using System.Globalization;
using System.Net;

namespace AppointmentApi
{
    public static class Extensions
    {

        public static AppointmentRequest AsDto(this Appointment appointment)
        {
           return new AppointmentRequest
           {
               StartTime = appointment.StartTime,
               EndTime = appointment.EndTime,
               Title = appointment.Title,
           };
        }


        public static HttpResponseException CustomException(this HttpResponseException CustomException, CustomError error, HttpStatusCode statusCode)
        {
            return new HttpResponseException((int)statusCode, error);
        }

        public static void Validate<TObject, TValidator>(this TObject instance)
            where TObject : class
            where TValidator : AbstractValidator<TObject>, new()
            {
                var validator = new TValidator();
                var results = validator.Validate(instance);
                if (!results.IsValid){
                    CustomError error = new CustomError { Message = "End time must be greater than Start Time." };
                    foreach (var failure in results.Errors)
                    {
                        if (failure.ErrorMessage.IndexOf("MM/DD/YYYY", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            error.Message = "Date format should be in MM/DD/YYYY";
                            List<CustomError> errorList = new(){error};
                            throw new HttpResponseException((int)HttpStatusCode.BadRequest, errorList);
                        }
                        else if(failure.ErrorMessage.IndexOf("empty", StringComparison.CurrentCultureIgnoreCase)!= -1)
                        {
                            error.Message = "Title cannot be empty";
                            List<CustomError> errorList = new()
                            {
                                error,
                                new CustomError(){Message= "StartTime should be in UTC format"},
                                new CustomError(){Message = "EndTime should be in UTC format"}
                            };
                            throw new HttpResponseException((int)HttpStatusCode.BadRequest,errorList);
                        }
                        else if (failure.ErrorMessage.IndexOf("greater", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            error.Message = "End time must be greater than Start Time.";
                            throw new HttpResponseException(400, error);
                        }
                        else if (failure.ErrorMessage.IndexOf("same date", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            error.Message = "Appointment can only be set for same day endTime and StartTime should have same date";
                            throw new HttpResponseException(400, error);
                        }
                        else
                        {
                            error.Message = "Server Error";
                            throw new HttpResponseException((int)HttpStatusCode.BadRequest, error);
                        }
                    }

                }
            }


        public static string GetSystemDateFormat(this DateTime dateTime)
        {
            // Get the current culture.
            CultureInfo culture = CultureInfo.CurrentCulture;

            // Get the short date format for the current culture.
            string dateFormat = culture.DateTimeFormat.ShortDatePattern;

            // Return the date format.
            return dateFormat;
        }

    }
}