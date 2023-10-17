using AppointmentApi.Models;

namespace  AppointmentApi.Buisness
{
    public interface IAppointmentBL 
    {
        public IEnumerable<Appointment> GetAppointments(); // Not required

        public IEnumerable<Appointment> GetAppointmentsBydate(string date); // change to data layer type

        public Appointment GetAppointment(Guid id); // Not required

        public bool DeleteAppointment(Guid id);

        public string CreateAppointment(AppointmentDto appointmentDto); // change dto toopenspec AppointmentREquest
    }
}