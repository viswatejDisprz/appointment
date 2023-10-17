using AppointmentApi.Models;

namespace  AppointmentApi.Buisness
{
    public interface IAppointmentBL 
    {
        public IEnumerable<Appointment> GetAppointments();

        public IEnumerable<Appointment> GetAppointmentsBydate(DateOnly date);

        public Appointment GetAppointment(Guid id);

        public void DeleteAppointment(Guid id);

        public string CreateAppointment(Appointment appointmentDto);
    }
}