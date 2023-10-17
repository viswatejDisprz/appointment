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

    IEnumerable<Appointment> GetAppointmentsBydate(DateOnly date);

    string CreateAppointment(Appointment appointment);

    void DeleteAppointment(Guid id);
  }
}