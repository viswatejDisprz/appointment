using AppointmentApi.Models;

public class HttpResponseException : Exception
{
    public int Status { get; set; } = 500;

    public object Value { get; set; }

    public HttpResponseException(int status, CustomError? error=null)
    {
        Value = error;
        Status = status;
    }

    public HttpResponseException(int status, List<CustomError> errorList)
    {
        Value = errorList;
        Status = status;
    }
}