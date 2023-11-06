using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
using AppointmentApi.DataAccess;

namespace Appointment_copy.Tests
{
    public class AppointmentBLTests
    {
        private Mock<IAppointmentDL> mockAppointmentDL;
        private AppointmentBL appointmentBL;

        public AppointmentBLTests()
        {
            mockAppointmentDL = new Mock<IAppointmentDL>();
            appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
        }

        [Fact]
        public void TestGetAppointments_ReturnsMatchingAppointments()
        {
            // Arrange
            var appointmentDateRequest = new AppointmentDateRequest { Date = new DateOnly(2023, 10, 15) };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/15 10:00"), EndTime = DateTime.Parse("2023/10/15 11:00") }
            };
            mockAppointmentDL.Setup(x => x.GetAppointments(null, appointmentDateRequest.Date)).Returns(appointments);

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
        // order
        [Theory]
        [InlineData("08:45", "09:45")]
        [InlineData("09:15", "10:15")]
        public void TestCreateAppointment_Throws_conflictError(string startTime, string endTime)
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse($"2023/10/3 {startTime}"),
                EndTime = DateTime.Parse($"2023/10/3 {endTime}")
            };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 09:00"), EndTime = DateTime.Parse("2023/10/3 10:00") },
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 10:00"), EndTime = DateTime.Parse("2023/10/3 11:00") },
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 11:00"), EndTime = DateTime.Parse("2023/10/3 12:00") }
            };
            DateOnly dateOnly = new DateOnly(appointmentRequest.StartTime.Year, appointmentRequest.StartTime.Month, appointmentRequest.StartTime.Day);
            mockAppointmentDL.Setup(x => x.GetAppointments(null, dateOnly)).Returns(appointments);

            // Act & Assert
            Assert.Throws<HttpResponseException>(() => appointmentBL.CreateAppointment(appointmentRequest));
            mockAppointmentDL.Verify(x => x.GetAppointments(null, dateOnly), Times.Once);
        }

        [Fact]
        public void TestDeleteAppointment_CallsDeleteMethodInDL()
        {
            // Arrange
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

        [Fact]
        public void TestDeleteAppointment_checkss_NotFound_ID_DeleteMethod()
        {
            // Arrange
            var id = Guid.NewGuid();
            var appointments = new List<Appointment>();
            mockAppointmentDL.Setup(x => x.GetAppointments(id, null)).Returns(appointments);

            // Act & Assert
            Assert.Throws<HttpResponseException>(() => appointmentBL.DeleteAppointment(id));
            mockAppointmentDL.Verify(m => m.GetAppointments(It.IsAny<Guid>(), null), Times.Once);
        }
    }
}
