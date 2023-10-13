using System;
using System.Collections.Generic;
namespace AppointmentApi.AppointmentBL
{
  public interface IAppointmentRepository
  {
    Appointment GetAppointment(Guid id);
    IEnumerable<Appointment> GetAppointments();

    void CreateAppointment(Appointment appointment);

    void DeleteAppointment(Guid id);
  }
}