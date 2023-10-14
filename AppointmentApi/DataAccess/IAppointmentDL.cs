using System;
using System.Collections.Generic;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
namespace AppointmentApi.DataAccess
{
  public interface IAppointmentDL
  {
    Appointment GetAppointment(Guid id);
    IEnumerable<Appointment> GetAppointments();

    IEnumerable<Appointment> GetAppointmentsBydate(string date);

    string CreateAppointment(AppointmentDto appointmentDto);

    void DeleteAppointment(Guid id);
  }
}