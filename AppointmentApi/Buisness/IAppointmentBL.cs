using AppointmentApi.Models;

namespace  AppointmentApi.Buisness
{
    public interface IAppointmentBL 
    {
        public IEnumerable<Appointment> GetAppointments();

        public IEnumerable<Appointment> GetAppointmentsBydate(string date);

        public Appointment GetAppointment(Guid id);

        public bool DeleteAppointment(Guid id);

        public string CreateAppointment(AppointmentDto appointmentDto);
    }
}