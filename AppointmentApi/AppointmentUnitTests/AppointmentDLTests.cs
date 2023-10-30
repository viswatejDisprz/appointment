using Xunit;
using AppointmentApi.Models;
using AppointmentApi.Buisness;

namespace Appointment_copy.Tests
{
    public class AppointmentDLTests
    {
        [Fact]
        public void TestGetAppointments_WithValidId_ReturnsMatchingAppointments()
        {
            // Arrange
            var appointmentDL = new AppointmentDL();
            var appointmentId = Guid.NewGuid();
            AppointmentRequest appointmentRequest = new AppointmentRequest
            {
                Title = "Go To Gym",
                StartTime = new DateTime(2023, 10, 15),
                EndTime = new DateTime(2023, 10, 15)
            };

            var createdAppointmentId = appointmentDL.CreateAppointment(appointmentRequest);
            // Act
            var result = appointmentDL.GetAppointments(createdAppointmentId);

            // Assert
            Assert.Single(result);
            Assert.Equal(createdAppointmentId, result[0].Id);
        }

        [Fact]
        public void TestCreateAppointment_ReturnsValidGuid()
        {
            // Arrange
            var appointmentDL = new AppointmentDL();
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = new DateTime(2023, 10, 3),
                EndTime = new DateTime(2023, 10, 4)
            };

            // Act
            var result = appointmentDL.CreateAppointment(appointmentRequest);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void TestDeleteAppointment_RemovesAppointmentFromList()
        {
            // Arrange
            var appointmentDL = new AppointmentDL();
            var appointmentId = Guid.NewGuid();
            AppointmentRequest appointmentRequest = new AppointmentRequest
            {
                Title = "Test Appointment",
                StartTime = new DateTime(2023, 10, 1),
                EndTime = new DateTime(2023, 10, 2)
            };
            var createdAppointmentId = appointmentDL.CreateAppointment(appointmentRequest);

            // Act
            appointmentDL.DeleteAppointment(createdAppointmentId);
            var result = appointmentDL.GetAppointments(createdAppointmentId);

            // Assert
            Assert.Empty(result);
        }
    }
}