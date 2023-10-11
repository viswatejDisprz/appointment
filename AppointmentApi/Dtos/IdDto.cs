using System;
using AppointmentApi.Models;
namespace AppointmentApi.Dtos
{
    public class IdDto: IPostDto
    {
        public Guid Id { get; set; }
    }
}