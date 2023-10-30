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
        public void CreateAppointment_ShouldCreateNewAppointment()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023-10-29 12:00:00"), EndTime = DateTime.Parse("2023-10-29 13:00:00"), Title="Jogging" };
            var expectedAppointmentId = Guid.NewGuid();

            _appointmentDLMock.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedAppointmentId);

            // Act
            var appointmentId = _appointmentBL.CreateAppointment(appointmentRequest);

            // Assert
            Assert.Equal(expectedAppointmentId, appointmentId);
        }

        [Fact]
        public void TestDeleteAppointment()
        {
            // Arrange
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var mockappointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var id = Guid.NewGuid();

            // Act
            mockappointmentBL.DeleteAppointment(id);

            // Assert
            mockAppointmentDL.Verify(x => x.DeleteAppointment(id), Times.Once);
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
