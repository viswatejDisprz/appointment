using AppointmentApi.Buisness;
using AppointmentApi.Models;

namespace AppointmentApiTests
{
    public class AppointmentDLTests
    {
        [Fact]
        public void GetAppointments_ValidDate_ReturnsMatchingAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { StartTime = new DateTime(2023, 10, 15, 23, 00, 00), EndTime = new DateTime(2023, 10, 15, 23, 59, 00), Title = "go to Gym", Id = Guid.NewGuid() },
                new Appointment { StartTime = new DateTime(2023, 10, 15, 13, 00, 00), EndTime = new DateTime(2023, 10, 15, 13, 59, 00), Title = "go for a walk", Id = Guid.NewGuid() },
                new Appointment { StartTime = new DateTime(2023, 10, 15, 19, 00, 00), EndTime = new DateTime(2023, 10, 15, 19, 59, 00), Title = "go for cohee", Id = Guid.NewGuid() }
            };
            var appointmentDL = new AppointmentDL();

            // Act
            var result = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

            // Assert
            Assert.Equal(appointments.Count, result.Count);
            foreach (var appointment in appointments)
            {
                Assert.Contains(result, app => app.StartTime == appointment.StartTime && app.EndTime == appointment.EndTime && app.Title == appointment.Title);
            }
        }
    }
}
