using System;
using AppointmentApi.Models;
namespace AppointmentApi.Dtos
{
    public class IdDto: IPostDto
    {
        public Guid Id { get; set; }
        public List<Appointment> k1 {get; set;}
    }
}