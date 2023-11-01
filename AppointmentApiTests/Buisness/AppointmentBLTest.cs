using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
using AppointmentApi.DataAccess;

namespace Appointment_copy.Tests
{
    public class AppointmentBLTests
    {
        [Fact]
        public void TestGetAppointments_ReturnsMatchingAppointments()
        {
            // Arrange
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentDateRequest = new AppointmentDateRequest { Date = new DateOnly(2023, 10, 15) };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = new DateTime(2023, 10, 15), EndTime = new DateTime(2023, 10, 15) }
            };
            mockAppointmentDL.Setup(x => x.GetAppointments(null, appointmentDateRequest.Date)).Returns(appointments);
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);

            // Act
            var result = appointmentBL.GetAppointments(appointmentDateRequest);

            // Assert
            Assert.Single(result);
            Assert.Equal("Go To Gym", result[0].Title);
        }

        [Fact]
        public void TestCreateAppointment_ReturnsValidGuid()
        {
            // Arrange
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse("2023/10/3 11:00"),
                EndTime = DateTime.Parse("2023/10/3 12:00")
            };
            var expectedGuid = Guid.NewGuid();
            mockAppointmentDL.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedGuid);

            // Act
            var result = appointmentBL.CreateAppointment(appointmentRequest);

            // Assert
            Assert.Equal(expectedGuid, result);
        }

        [Fact]
        public void TestDeleteAppointment_CallsDeleteMethodInDL()
        {
            // Arrange
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse("2023/10/3 11:00"),
                EndTime = DateTime.Parse("2023/10/3 12:00")
            };
            var id = appointmentBL.CreateAppointment(appointmentRequest);
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = new DateTime(2023, 10, 15), EndTime = new DateTime(2023, 10, 15) }
            };
            mockAppointmentDL.Setup(x => x.GetAppointments(id, null)).Returns(appointments);
            mockAppointmentDL.Setup(x => x.DeleteAppointment(id));

            // Act
            appointmentBL.DeleteAppointment(id);

            // Assert
            mockAppointmentDL.Verify(x => x.DeleteAppointment(id), Times.Once);
        }

    }
}