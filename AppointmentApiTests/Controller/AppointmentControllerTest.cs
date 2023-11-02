using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Controllers;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_copy.Tests
{
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentBL> mockAppointmentBL;
        private AppointmentController appointmentController;

        public AppointmentControllerTests()
        {
            mockAppointmentBL = new Mock<IAppointmentBL>();
            appointmentController = new AppointmentController(mockAppointmentBL.Object);
        }

        [Fact]
        public void TestGetAppointments_ReturnsListOfAppointments()
        {
            // Arrange
            var appointmentDateRequest = new AppointmentDateRequest { Date = new DateOnly(2023, 10, 15) };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = new DateTime(2023, 10, 15), EndTime = new DateTime(2023, 10, 15) }
            };
            mockAppointmentBL.Setup(x => x.GetAppointments(appointmentDateRequest)).Returns(appointments);

            // Act
            var result = appointmentController.GetAppointments(appointmentDateRequest);

            // Assert
            Assert.Collection(result, item =>
            {
                Assert.Equal("Go To Gym", item.Title);
            });
        }

        [Fact]
        public void TestCreateAppointment_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = new DateTime(2023, 10, 3),
                EndTime = new DateTime(2023, 10, 4)
            };
            var expectedGuid = Guid.NewGuid();            

            // Act
            mockAppointmentBL.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedGuid);
            var result = appointmentController.CreateAppointment(appointmentRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
            var model = Assert.IsType<GuidValueResult>(createdAtActionResult.Value);
            Assert.Equal(expectedGuid, model.Id);
        }

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
