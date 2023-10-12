using System;
using AppointmentApi.Models;
namespace AppointmentApi.Dtos
{
    public class IdDto: ResponseDto
    {
        public Guid Id { get; set; }
    }
}