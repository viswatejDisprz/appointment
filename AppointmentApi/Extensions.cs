using AppointmentApi.Models;
using FluentValidation;
using AppointmentApi;

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

        public static void Validate<TObject, TValidator>(this TObject instance)
            where TObject : class
            where TValidator : AbstractValidator<TObject>, new()
        {
            var validator = new TValidator();
            var results = validator.Validate(instance);
            if (!results.IsValid)
            {
                 results.Errors.CustomException();
            }
        }


        public static string ReplaceDynamicDateFormat(this string xmlComments)
        {
            string dynamicDateFormat = GetDynamicDateFormat();

            return xmlComments.Replace("{DynamicDateFormat}", dynamicDateFormat);
        }

        private static string GetDynamicDateFormat()
        {
            string dynamicDateFormat = Environment.OSVersion.Platform == PlatformID.Unix ? "DD/MM/YYYY" : "MM/DD/YYYY";
            return dynamicDateFormat;
        }
    }
}