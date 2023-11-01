using System;
using FluentValidation.TestHelper;
using AppointmentApi.validators;
using AppointmentApi.Models;

public class AppointmentDateRequestValidatorTests
{
    private AppointmentDateRequestValidator _validator;

    public AppointmentDateRequestValidatorTests()
    {
        _validator = new AppointmentDateRequestValidator();
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
        var appointmentDateRequest = new AppointmentDateRequest
        {
            Date = DateOnly.Parse("2023-11-01")
        };

        var result = _validator.Validate(appointmentDateRequest);

        Assert.True(result.IsValid);
    }

}
