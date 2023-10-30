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
        
        public static CustomError DynamicErrorMessage(string errorString)
        {
            return new CustomError()
            {
                Message = errorString
            };
        }

        public static void Validate<TObject, TValidator>(this TObject instance)
            where TObject : class
            where TValidator : AbstractValidator<TObject>, new()
            {
                var validator = new TValidator();
                var results = validator.Validate(instance);
                if (!results.IsValid){
                   results.Errors.ForEach(failure =>
                    {
                        if (failure.ErrorMessage.IndexOf("MM/DD/YYYY", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            List<CustomError> errorList = new(){DynamicErrorMessage(failure.ErrorMessage.ToString())};
                            throw new HttpResponseException((int)HttpStatusCode.BadRequest, errorList);
                        }
                        else if(failure.ErrorMessage.IndexOf("empty", StringComparison.CurrentCultureIgnoreCase)!= -1)
                        {
                            List<CustomError> errorList = new()
                            {
                                DynamicErrorMessage(failure.ErrorMessage),
                                DynamicErrorMessage("StartTime should be in UTC format"),
                                DynamicErrorMessage("EndTime should be in UTC format")
                            };
                            throw new HttpResponseException((int)HttpStatusCode.BadRequest,errorList);
                        }
                        else
                        {
                            throw new HttpResponseException(400, DynamicErrorMessage(failure.ErrorMessage));
                        }

                    });

                }
            }


        public static string ReplaceDynamicDateFormat(this string xmlComments)
            {
                // Logic to determine the dynamic date format based on the system environment
                string dynamicDateFormat = GetDynamicDateFormat(); // Replace with your logic to determine the dynamic date format

                return xmlComments.Replace("{DynamicDateFormat}", dynamicDateFormat);
            }

        private static string GetDynamicDateFormat()
            {
                // Logic to determine the dynamic date format based on the system environment
                // For example, you can use a conditional check based on the operating system
                string dynamicDateFormat = Environment.OSVersion.Platform == PlatformID.Unix ? "DD/MM/YYYY" : "MM/DD/YYYY";
                return dynamicDateFormat;
            }
    }
}