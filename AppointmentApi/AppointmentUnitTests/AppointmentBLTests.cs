using AppointmentApi.Buisness;
using AppointmentApi.DataAccess;
using AppointmentApi.Models;
using Moq;
using Xunit;

namespace AppointmentApi.AppointmentUnitTests
{
    public class AppointmentBLTests
    {
        private Mock<IAppointmentDL> _appointmentDLMock;
        private AppointmentBL _appointmentBL;

        public AppointmentBLTests()
        {
            _appointmentDLMock = new Mock<IAppointmentDL>();
            _appointmentBL = new AppointmentBL(_appointmentDLMock.Object);
        }

        [Fact]
        public void GetAppointments_ShouldReturnListOfAppointments()
        {
            // Arrange
            var appointmentDateRequest = new AppointmentDateRequest { Date = DateOnly.Parse("2023-10-29") };
            var expectedAppointments = new List<Appointment> { new Appointment { Id = Guid.NewGuid(), StartTime = DateTime.Parse("2023-10-29 10:00:00"), EndTime = DateTime.Parse("2023-10-29 11:00:00") } };

            // Act
            var appointments = _appointmentBL.GetAppointments(appointmentDateRequest);

            // Assert
            Assert.Equal(expectedAppointments, appointments);
        }

        [Fact]
        public void CreateAppointment_ShouldCreateNewAppointment()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023-10-29 12:00:00"), EndTime = DateTime.Parse("2023-10-29 13:00:00") };
            var expectedAppointmentId = Guid.NewGuid();

            _appointmentDLMock.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedAppointmentId);

            // Act
            var appointmentId = _appointmentBL.CreateAppointment(appointmentRequest);

            // Assert
            Assert.Equal(expectedAppointmentId, appointmentId);
        }

        [Fact]
        public void DeleteAppointment_ShouldDeleteExistingAppointment()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();

            // Act
            _appointmentBL.DeleteAppointment(appointmentId);

            // Assert
            _appointmentDLMock.Verify(x => x.DeleteAppointment(appointmentId), Times.Once);
        }

        [Fact]
        public void DeleteAppointment_ShouldThrowHttpResponseExceptionIfAppointmentNotFound()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            _appointmentDLMock.Setup(x => x.GetAppointments(appointmentId, null)).Returns(new List<Appointment>());

            // Act
            Action act = () => _appointmentBL.DeleteAppointment(appointmentId);

            // Assert
            Assert.Throws<HttpResponseException>(act);
        }
    }
}
