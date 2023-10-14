using System;
using System.Collections.Generic;
using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using AppointmentApi.Models;

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