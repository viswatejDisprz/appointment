// This is a Guid value result Dto which appears upon appointment creation
using System;
namespace AppointmentApi.Models
{
    public class GuidValueResult: IResponseDto
    {
        public Guid Id { get; set; }
    }
}