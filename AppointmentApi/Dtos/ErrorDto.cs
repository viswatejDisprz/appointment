namespace AppointmentApi.Dtos
{
    public class ErrorDto:IPostDto
    {
        public string Error{get; set;}
        public int ErrorCode {get; set;}
    }
}