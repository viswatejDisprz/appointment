using System;
using System.Collections.Generic;
using AppointmentApi.Models;
using AppointmentApi.Dtos;

namespace AppointmentApi
{
    public static class Extensions
    {
        public static AppointmentDto AsDto(this Appointment appointment)
        {
           return new AppointmentDto
           {
               StartTime = appointment.StartTime,
               EndTime = appointment.EndTime,
               Title = appointment.Title,
            //    Id = appointment.Id
           };
        }
    }
}