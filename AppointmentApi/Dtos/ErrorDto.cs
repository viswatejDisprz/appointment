namespace AppointmentApi.Dtos
{
    public class ErrorDto:ResponseDto
    {
        public string Error{get; set;}
        public int ErrorCode {get; set;}
    }
}