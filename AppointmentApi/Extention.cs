using System;
using System.Collections.Generic;
using AppointmentApi.Buisness;
using AppointmentApi.DataAccess;

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
           };
        }
    }
}