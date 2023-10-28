// An extension is defined to conevert appointmentDto to appointment which includes Id
using AppointmentApi.Models;
using AppointmentApi.validators;
using FluentValidation;
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
        //    CustomError error = new CustomError(){Message = firstError.ErrorMessage};

            return new HttpResponseException((int)statusCode, error);
        }

        public static void Validate<TObject, TValidator>(this TObject instance)
            where TObject : class
            where TValidator : AbstractValidator<TObject>, new()
            {
                var validator = new TValidator();
                var results = validator.Validate(instance);
                if (!results.IsValid){
                    CustomError error = new CustomError(){Message = results.Errors.FirstOrDefault().ErrorMessage};

                     throw new HttpResponseException((int)HttpStatusCode.BadRequest, error);
                }
            }



    }
}