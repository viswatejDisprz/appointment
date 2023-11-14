using AppointmentApi.Models;
using FluentValidation;

namespace AppointmentApi
{
    public static class Extensions
    {

        // Generic validator
        public static void Validate<TObject, TValidator>(this TObject instance)
            where TObject : class
            where TValidator : AbstractValidator<TObject>, new()
        {
            var validator = new TValidator();
            var results = validator.Validate(instance);
            if (!results.IsValid)
                results.Errors.CustomException();
        }

        // Response exception extensions for list and object type
        public static HttpResponseException CustomException(this CustomError error, int statusCode)
        {
            return new HttpResponseException(statusCode, error);
        }

        public static HttpResponseException CustomException(this List<CustomError> error, int statusCode)
        {
            return new HttpResponseException(statusCode, error);
        }
    }
}