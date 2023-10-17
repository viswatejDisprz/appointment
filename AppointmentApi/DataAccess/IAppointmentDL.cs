using System;
using System.Collections.Generic;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
namespace AppointmentApi.DataAccess
{
  public interface IAppointmentDL
  {
    Appointment GetAppointment(Guid id);
    List<Appointment> GetAppointments(); // Take parameter here only change to return type List Guid? id=null, DateOnly? date=null

    IEnumerable<Appointment> GetAppointmentsBydate(DateOnly date); // Not required

    string CreateAppointment(Appointment appointment); // remove string return type

    void DeleteAppointment(Guid id);
  }
}