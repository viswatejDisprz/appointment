using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Controllers;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Mvc;
using MockAppointmentApiTests;

namespace Appointment_copy.Tests
{
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentBL> mockAppointmentBL;
        private AppointmentController appointmentController;
        private MockAppointments mock;
        
        public AppointmentControllerTests()
        {
            mockAppointmentBL = new Mock<IAppointmentBL>();
            appointmentController = new AppointmentController(mockAppointmentBL.Object);
            mock = new MockAppointments();
        }


        /// <summary>
        /// Get Appointment Testing
        /// </summary>
        [Fact]
        public void TestGetAppointments_ReturnsListOfAppointments()
        {
            // Arrange
            var appointmentDateRequest = mock.aptDateRequest();
            mock.GetAppointmentMock(mockAppointmentBL,appointmentDateRequest: appointmentDateRequest);
            
            // Act
            var result = appointmentController.GetAppointments(appointmentDateRequest);

            // Assert
            Assert.Single(result);
            Assert.Equal("Go To Gym", result[0].Title);
        }



        /// <summary>
        /// Create Appointment Testing
        /// </summary>
        [Fact]
        public void TestCreateAppointment_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var appointmentRequest = mock.aptRequest();
            var expectedGuid = Guid.NewGuid();            

            // Act
            mockAppointmentBL.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedGuid);
            var result = appointmentController.CreateAppointment(appointmentRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
            var model = Assert.IsType<GuidValueResult>(createdAtActionResult.Value);
            Assert.Equal(expectedGuid, model.Id);
        }


        /// <summary>
        /// Delete Appointment Testing
        /// </summary>
        [Fact]
        public void TestDeleteAppointment_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = appointmentController.DeleteAppointment(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
