using AppointmentApi.Models;
using AppointmentApi.validators;

public class AppointmentRequestValidatorTests
{
    private AppointmentRequestValidator _validator;

    public AppointmentRequestValidatorTests()
    {
        _validator = new AppointmentRequestValidator();
    }

    [Fact]
    public void Should_fail_validation_when_all_fields_are_empty()
    {
        var appointmentRequest = new AppointmentRequest();

        var result = _validator.Validate(appointmentRequest);

        Assert.False(result.IsValid);
        Assert.Contains("Appointment Title should not be empty", result.Errors.FirstOrDefault().ErrorMessage);
        Assert.Contains("Appointment StartTime should not be empty", result.Errors.Skip(1).First().ErrorMessage);
        Assert.Contains("Appointment EndTime should not be empty", result.Errors.Skip(2).First().ErrorMessage);
    }

    [Fact]
    public void Should_pass_validation_when_all_fields_are_valid()
    {
        var appointmentRequest = new AppointmentRequest
        {
            Title = "My Appointment",
            StartTime = DateTime.Parse("2023-11-01 10:00:00"),
            EndTime = DateTime.Parse("2023-11-01 11:00:00")
        };

        var result = _validator.Validate(appointmentRequest);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_fail_validation_when_endTime_is_less_than_startTime()
    {
        var appointmentRequest = new AppointmentRequest
        {
            Title = "My Appointment",
            StartTime = DateTime.Parse("2023-11-01 11:00:00"),
            EndTime = DateTime.Parse("2023-11-01 10:00:00")
        };

        var result = _validator.Validate(appointmentRequest);

        Assert.False(result.IsValid);
        Assert.Contains("End time must be greater than Start Time.", result.Errors.FirstOrDefault().ErrorMessage);
    }

    [Fact]
    public void Should_fail_validation_when_endTime_and_startTime_are_not_on_same_day()
    {
        var appointmentRequest = new AppointmentRequest
        {
            Title = "My Appointment",
            StartTime = DateTime.Parse("2023-11-01 10:00:00"),
            EndTime = DateTime.Parse("2023-11-02 11:00:00")
        };

        var result = _validator.Validate(appointmentRequest);

        Assert.False(result.IsValid);
        Assert.Contains("Appointment can only be set for same day endTime and StartTime should have same date", result.Errors.FirstOrDefault().ErrorMessage);
    }
}

