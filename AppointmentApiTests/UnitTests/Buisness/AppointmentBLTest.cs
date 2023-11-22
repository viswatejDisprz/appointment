using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
using AppointmentApi.DataAccess;
using MockAppointmentApiTests;

namespace Appointment_copy.Tests
{
    public class AppointmentBLTests
    {
        private Mock<IAppointmentDL> mockAppointmentDL;
        private AppointmentBL appointmentBL;
        private MockAppointments mock;

        public AppointmentBLTests()
        {
            mockAppointmentDL = new Mock<IAppointmentDL>();
            appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            mock = new MockAppointments();
        }
 
        /// <summary>
        /// Get Request Testing
        /// </summary>

        [Fact]
        public void TestGetAppointments_ReturnsMatchingAppointments()
        {
            // Arrange
            mock.GetAppointmentMock(mockAppointmentDL:mockAppointmentDL);
            
            // Act
            var result = appointmentBL.GetAppointments(mock.aptDateRequest());

            // Assert
            Assert.Single(result);
            Assert.Equal("Go To Gym", result[0].Title);
        }


        /// <summary>
        ///  Create Appointment Testing 
        /// </summary>

        [Fact]
        public void TestCreateAppointment_ReturnsValidGuid()
        {
            // Arrange
            var appointmentRequest = mock.aptRequest();

            var expectedGuid = Guid.NewGuid(); 
            mock.GetAppointmentMock(mockAppointmentDL:mockAppointmentDL); 
            mockAppointmentDL.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedGuid);

            // Act
            var result = appointmentBL.CreateAppointment(appointmentRequest);

            // Assert
            Assert.Equal(expectedGuid, result);
        }
        


        [Theory]
        [InlineData("09:45", "10:15")]
        [InlineData("10:15", "10:45")]
        [InlineData("08:00", "11:15")]
        [InlineData("10:10", "10:20")]
        public void TestCreateAppointment_Throws_conflictError(string startTime, string endTime)
        {
            // Arrange
            var appointmentRequest = mock.aptRequest(startTime,endTime);

            DateOnly dateOnly = DateOnly.FromDateTime(appointmentRequest.StartTime);

            mock.GetAppointmentMock(mockAppointmentDL:mockAppointmentDL);

            // Act & Assert
            Assert.Throws<HttpResponseException>(() => appointmentBL.CreateAppointment(appointmentRequest));
        }



        /// <summary>
        /// Delete Appointment Testing
        /// </summary>
        [Fact]
        public void TestDeleteAppointment_CallsDeleteMethodInDL()
        {
            // Arrange
            mockAppointmentDL.Setup(x => x.GetAppointment(It.IsAny<Guid>())).Returns(mock.appointments.FirstOrDefault());
            mockAppointmentDL.Setup(x => x.DeleteAppointment(It.IsAny<Guid>()));

            // Act
            var id = Guid.NewGuid();
            appointmentBL.DeleteAppointment(id);

            // Assert
            mockAppointmentDL.Verify(x => x.DeleteAppointment(id), Times.Once);
        }

        [Fact]
        public void TestDeleteAppointment_checks_NotFound_ID_DeleteMethod()
        {
            // Arrange
            var id = Guid.NewGuid();
            var appointments = new List<Appointment>();
            mockAppointmentDL.Setup(x => x.GetAppointment(id)).Returns((Appointment)null);

            // Act & Assert
            Assert.Throws<HttpResponseException>(() => appointmentBL.DeleteAppointment(id));
            mockAppointmentDL.Verify(m => m.GetAppointment(It.IsAny<Guid>()), Times.Once);
        }
    }
}
