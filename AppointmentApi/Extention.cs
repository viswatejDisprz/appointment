// An extension is defined to conevert appointmentDto to appointment which includes Id
using System;
using System.Collections.Generic;
using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using AppointmentApi.Models;

namespace AppointmentApi
{
    public static class Extensions
    {
        public static AppointmentRequest AsDto(this Appointment appointment)
        {
           return new AppointmentRequest
           {
               StartTime = appointment.StartTime,
               EndTime = appointment.EndTime,
               Title = appointment.Title,
           };
        }
    }
}