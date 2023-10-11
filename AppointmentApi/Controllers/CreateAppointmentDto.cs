using Microsoft.IdentityModel.Tokens;

namespace AppointmentApi.Dtos
{
    public class CreateAppointmentDto
    {
        public string Title {get; init;}
        public DateTime StartTime {get; init;}

        public DateTime EndTime {get; init;}

    }
}