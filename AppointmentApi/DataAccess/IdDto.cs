using System;
namespace AppointmentApi.DataAccess
{
    public class IdDto: ResponseDto
    {
        public Guid Id { get; set; }
    }
}