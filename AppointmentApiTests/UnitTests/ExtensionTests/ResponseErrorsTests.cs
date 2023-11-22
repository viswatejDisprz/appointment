
public class ResponseErrorsTests
{

    [Fact]
    public void BadRequest_should_return_a_CustomError_with_the_specified_message()
    {
        var message = "Invalid appointment request.";
        var error = ResponseErrors.BadRequest(message, 01);

        Assert.Equal(message, error.Message);
    }
}
