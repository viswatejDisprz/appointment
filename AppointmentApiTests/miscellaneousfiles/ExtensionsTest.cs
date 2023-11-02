using AppointmentApi.Models;
using AppointmentApi;
using System.Globalization;
using AppointmentApi.validators;
using Microsoft.AspNetCore.Http;
public class ExtensionsTests
{
    [Fact]
    public void Validate_should_throw_exception_if_validation_fails()
    {
        var appointmentRequest = new AppointmentRequest();

        Assert.Throws<HttpResponseException>(() => appointmentRequest.Validate<AppointmentRequest, AppointmentRequestValidator>());
    }

    [Fact]
    public void ReplaceDynamicDateFormat_ShouldReplaceDateFormat()
    {
        // Arrange
        string xmlComments = "Some example XML comments with {DynamicDateFormat}";

        // Act
        var replacedXmlComments = Extensions.ReplaceDynamicDateFormat(xmlComments);

        // Assert
        Assert.DoesNotContain("{DynamicDateFormat}", replacedXmlComments);
    }

    [Fact]
    public void GetDynamicDateFormat_should_return_the_current_culture_short_date_pattern()
    {
        var dynamicDateFormat = Extensions.GetDynamicDateFormat();

        var culture = CultureInfo.CurrentCulture;
        var shortDatePattern = culture.DateTimeFormat.ShortDatePattern;

        Assert.Equal(shortDatePattern, dynamicDateFormat);
    }

    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_with_the_InternalServer_status_code()
    {
        var error = new CustomError
        {
            Message = "This is a sample error message."
        };

        Assert.IsType<System.Func<HttpResponseException>>(() => error.CustomException(StatusCodes.Status500InternalServerError));
    }

    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_with_the_BadRequest_status_code()
    {
        var error = new CustomError
        {
            Message = "This is a sample error message."
        };
        var exception = error.CustomException(StatusCodes.Status400BadRequest);

        Assert.IsType<HttpResponseException>(exception);
    }

    [Fact]
    public void CustomException_should_throw_an_HttpResponseException_for_customError_list_with_the_specified_status_code()
    {
        var error = new CustomError
        {
            Message = "This is a sample error message."
        };
        var errorList = new List<CustomError>();

        Assert.IsType<System.Func<HttpResponseException>>(() => errorList.CustomException(StatusCodes.Status400BadRequest));
    }
}
