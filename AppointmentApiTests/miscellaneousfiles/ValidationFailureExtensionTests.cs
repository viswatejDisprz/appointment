using FluentValidation.Results;
using AppointmentApi;
using Microsoft.AspNetCore.Http;
using AppointmentApi.Models;
using AppointmentApi.validators;
using System.ComponentModel.DataAnnotations;

public class ValidationFailureExtensionTests
{
    // [Fact]
    // public void CustomException_should_throw_an_HttpResponseException_with_a_400_Bad_Request_status_code()
    // {
    //     // Arrange
    //     var results = new List<ValidationFailure>();
    //     results.Add(new ValidationFailure("PropertyName", "ErrorMessage"));

    //     // Act
    //     results.CustomException();

    //     // Assert
    //     Assert.Throws<HttpResponseException>(() => results.CustomException());
    // }

    // [Fact]
    // public void CustomException_should_convert_each_ValidationFailure_to_a_CustomError()
    // {
    //     // Arrange
    //     var appointmentRequest = new AppointmentRequest(){ EndTime = DateTime.Parse("2023/10/15 11:00"), StartTime = DateTime.Parse("2023/10/15 12:00"), Title=""};
    //     FluentValidation.Results.ValidationResult r = new AppointmentRequestValidator().Validate(appointmentRequest);
    //     var results = new List<CustomError>();

    //     // Act
    //     r.Errors.CustomException();

    //     // Assert
    //     Assert.Throws<HttpResponseException>(() => results.CustomException(StatusCodes.Status400BadRequest));
    // }
}
