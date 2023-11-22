using AppointmentApi.validators;
using AppointmentApi.Models;
using MockAppointmentApiTests;

public class AppointmentDateRequestValidatorTests
{
    private AppointmentDateRequestValidator _validator;

    private MockAppointments mock;

    public AppointmentDateRequestValidatorTests()
    {
        _validator = new AppointmentDateRequestValidator();
        mock = new MockAppointments();
    }

    [Fact]
    public void Should_fail_validation_when_date_is_empty()
    {
        var appointmentDateRequest = new AppointmentDateRequest();

        var result = _validator.Validate(appointmentDateRequest);

        Assert.False(result.IsValid);
        Assert.Contains("Please enter the Correct date format in", result.Errors.FirstOrDefault().ErrorMessage);
    }

    [Fact]
    public void Should_pass_validation_when_date_is_in_correct_format()
    {
        var appointmentDateRequest = mock.aptDateRequest();

        var result = _validator.Validate(appointmentDateRequest);

        Assert.True(result.IsValid);
    }
}
