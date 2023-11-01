using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Controllers;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_copy.Tests
{
    public class AppointmentControllerTests
    {
        [Fact]
        public void TestGetAppointments_ReturnsListOfAppointments()
        {
            // Arrange
            var mockAppointmentBL = new Mock<IAppointmentBL>();
            var appointmentDateRequest = new AppointmentDateRequest { Date = new DateOnly(2023, 10, 15) };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = new DateTime(2023, 10, 15), EndTime = new DateTime(2023, 10, 15) }
            };
            mockAppointmentBL.Setup(x => x.GetAppointments(appointmentDateRequest)).Returns(appointments);
            var appointmentController = new AppointmentController(mockAppointmentBL.Object);

            // Act
            var result = appointmentController.GetAppointments(appointmentDateRequest);

            // Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.Single(result);
            Assert.Equal("Go To Gym", result[0].Title);
        }

        [Fact]
        public void TestCreateAppointment_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var mockAppointmentBL = new Mock<IAppointmentBL>();
            var appointmentController = new AppointmentController(mockAppointmentBL.Object);
            var appointmentRequest = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = new DateTime(2023, 10, 3),
                EndTime = new DateTime(2023, 10, 4)
            };
            var expectedGuid = Guid.NewGuid();
            mockAppointmentBL.Setup(x => x.CreateAppointment(appointmentRequest)).Returns(expectedGuid);

            // Act
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
            var mockAppointmentBL = new Mock<IAppointmentBL>();
            var appointmentController = new AppointmentController(mockAppointmentBL.Object);
            var id = Guid.NewGuid();

            // Act
            var result = appointmentController.DeleteAppointment(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}