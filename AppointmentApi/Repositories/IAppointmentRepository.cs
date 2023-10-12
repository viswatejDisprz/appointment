using System;
using System.Collections.Generic;
using AppointmentApi.Models;
namespace AppointmentApi.Repositories
{
  public interface IAppointmentRepository
  {
    Appointment GetAppointment(Guid id);
    IEnumerable<Appointment> GetAppointments();

    void CreateAppointment(Appointment appointment);

    void DeleteAppointment(Guid id);
  }
}