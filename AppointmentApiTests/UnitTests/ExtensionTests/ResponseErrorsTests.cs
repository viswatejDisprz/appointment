
public class ResponseErrorsTests
{
    [Fact]
    public void NotFound_should_return_a_CustomError_with_the_correct_message()
    {
        var error = ResponseErrors.NotFound;

        Assert.Equal("Appointment not found", error.Message);
    }

    [Fact]
    public void BadRequest_should_return_a_CustomError_with_the_specified_message()
    {
        var message = "Invalid appointment request.";
        var error = ResponseErrors.BadRequest(message);

        Assert.Equal(message, error.Message);
    }
}
