using System.Net;
using AppointmentApi.Models;
using FluentValidation.Results;
namespace AppointmentApi;
public static class ValidationFailureExtension
{
    public static void CustomException(this List<ValidationFailure> results)
        {
            List<CustomError> list_errors = new();
            results.ForEach(failure => {
                list_errors.Add(ResponseErrors.BadRequest(failure.ErrorMessage));
            });
            throw list_errors.CustomException(StatusCodes.Status400BadRequest);
        }
}