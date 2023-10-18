using AppointmentApi.Models;

namespace  AppointmentApi.Buisness
{
    public interface IAppointmentBL 
    {
        // public IEnumerable<Appointment> GetAppointments(); // Not required

        public List<Appointment> GetAppointments(Guid? id,DateOnly? date); // change to data layer type

        // public Appointment GetAppointment(Guid id); // Not required

        public bool DeleteAppointment(Guid id);

        public string CreateAppointment(AppointmentRequest appointmentrequest); // change dto toopenspec AppointmentREquest
    }
}