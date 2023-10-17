using System;
using System.Collections.Generic;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
namespace AppointmentApi.DataAccess
{
  public interface IAppointmentDL
  {
    List<Appointment> GetAppointments(Guid? id=null, DateOnly? date=null); // Take parameter here only change to return type List Guid? id=null, DateOnly? date=null

    Guid CreateAppointment(Appointment appointment); // remove string return type

    void DeleteAppointment(Guid id);
  }
}