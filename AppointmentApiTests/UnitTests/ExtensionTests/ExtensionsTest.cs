using AppointmentApi.Models;
using AppointmentApi;
using AppointmentApi.validators;
using Microsoft.AspNetCore.Http;
using MockAppointmentApiTests;
public class ExtensionsTests
{
    private MockAppointments mock;

    public ExtensionsTests()
    {
        mock  = new MockAppointments();
    }
    
    [Fact]
    public void Validate_should_throw_exception_if_validation_fails()
    {
        var appointmentRequest = new AppointmentRequest();

        Assert.Throws<HttpResponseException>(() => appointmentRequest.Validate<AppointmentRequest, AppointmentRequestValidator>());
    }

    [Fact]
    public void Validate_should_throw_exception_if_validation_fails_DateRequest()
    {
        var appointmentDateRequest = new AppointmentDateRequest();

        Assert.Throws<HttpResponseException>(() => appointmentDateRequest.Validate<AppointmentDateRequest, AppointmentDateRequestValidator>());
    }
    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_with_the_InternalServer_status_code()
    {
        var error = mock.customError();

        Assert.IsType<System.Func<HttpResponseException>>(() => error.CustomException(StatusCodes.Status500InternalServerError));
    }

    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_with_the_BadRequest_status_code()
    {
        var error = mock.customError();

        var exception = error.CustomException(StatusCodes.Status400BadRequest);

        Assert.IsType<HttpResponseException>(exception);
    }

    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_for_customError_list_with_the_specified_status_code()
    {
        var error = mock.customError();
        
        var errorList = new List<CustomError>();

        Assert.IsType<System.Func<HttpResponseException>>(() => errorList.CustomException(StatusCodes.Status400BadRequest));
    }
}
