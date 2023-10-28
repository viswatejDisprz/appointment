using System.Net;
using AppointmentApi.Models;

public class HttpResponseException : Exception
{
    public int Status { get; set; } = 500;

    public object Value { get; set; } // display error in dynamic format

    public HttpResponseException(int status ,CustomError error)
    {
       Value =  error;
       Status = status;
    }

    public HttpResponseException(int status, List<CustomError> errorList)
    {
        Value = errorList;
        Status = status;
    }
}