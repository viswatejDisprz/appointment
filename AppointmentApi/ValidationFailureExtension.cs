using System.Net;
using AppointmentApi.Models;
using FluentValidation.Results;
namespace AppointmentApi;
public static class ValidationFailureExtension
{
    public static void CustomException(this List<ValidationFailure> results)
        {
            results.ForEach(failure =>
                 {
                     if (failure.ErrorMessage.IndexOf("MM/DD/YYYY", StringComparison.CurrentCultureIgnoreCase) != -1)
                     {
                         List<CustomError> errorList = new() { DynamicErrorMessage(failure.ErrorMessage.ToString()) };
                         throw new HttpResponseException((int)HttpStatusCode.BadRequest, errorList);
                     }
                     else if (failure.ErrorMessage.IndexOf("empty", StringComparison.CurrentCultureIgnoreCase) != -1)
                     {
                         List<CustomError> errorList = new()
                         {
                                DynamicErrorMessage(failure.ErrorMessage),
                                DynamicErrorMessage("StartTime should be in UTC format"),
                                DynamicErrorMessage("EndTime should be in UTC format")
                         };
                         throw new HttpResponseException((int)HttpStatusCode.BadRequest, errorList);
                     }
                     else
                     {
                         throw new HttpResponseException(400, DynamicErrorMessage(failure.ErrorMessage));
                     }

                 });
        }

        public static CustomError DynamicErrorMessage(string errorString)
            {
                return new CustomError()
                {
                    Message = errorString
                };
            }
}