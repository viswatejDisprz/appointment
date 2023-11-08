using AppointmentApi.Models;

namespace AppointmentApi.Buisness
{
    public interface IAppointmentBL
    {
        // Used to fetch appointments of the day
        // Takes AppointmentDateRequest Dto as an arg
        // Throws validatoin error if input is incorrect 
        // Returns a list of appointments if correct
        public List<Appointment> GetAppointments(AppointmentDateRequest appointmentDateRequest);

        // Used to create and add an appointment
        // Takes AppointmentRequest Dto as an argument
        // validates the input and also checks for conflicts
        // returns guid of the created appointment
        public void DeleteAppointment(Guid id);

        // Deletes an apppointment from the list
        // Takes id as an argument
        // throws error if not found
        // Deletes an appointment
        public Guid CreateAppointment(AppointmentRequest appointmentrequest);
    }
}