using AppointmentApi.Models;

namespace AppointmentApi.Buisness
{
    public interface IAppointmentBL
    {

        public List<Appointment> GetAppointments(AppointmentDateRequest appointmentDateRequest);

        public void DeleteAppointment(Guid id);

        public Guid CreateAppointment(AppointmentRequest appointmentrequest);
    }
}