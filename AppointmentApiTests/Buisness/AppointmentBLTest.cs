using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
using AppointmentApi.DataAccess;
using Microsoft.AspNetCore.Http;

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
        public void TestCreateAppointment_Throws_conflictError_startTime()
        {
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var appointmentRequest1 = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse("2023/10/3 09:00"),
                EndTime = DateTime.Parse("2023/10/3 10:20")
            };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 10:00"), EndTime = DateTime.Parse("2023/10/3 11:00") }
            };
            DateOnly dateOnly = new DateOnly(appointmentRequest1.StartTime.Year, appointmentRequest1.StartTime.Month, appointmentRequest1.StartTime.Day);
            mockAppointmentDL.Setup(x => x.GetAppointments(null, dateOnly)).Returns(appointments);
            var expectedGuid = Guid.NewGuid();
            mockAppointmentDL.Setup(x => x.CreateAppointment(appointmentRequest1)).Returns(expectedGuid);
            var errorDto = new CustomError() { Message = "Appoinment conflict" };
            // Act
            Assert.Throws<HttpResponseException>(() => appointmentBL.CreateAppointment(appointmentRequest1));

            // Assert
            mockAppointmentDL.Verify(x => x.GetAppointments(null,dateOnly),Times.Once);

        }



        [Fact]
        public void TestCreateAppointment_Throws_conflictError_endTime()
        {
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var appointmentRequest2 = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse("2023/10/3 10:20"),
                EndTime = DateTime.Parse("2023/10/3 11:00")
            };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 10:00"), EndTime = DateTime.Parse("2023/10/3 11:00") }
            };
            DateOnly dateOnly = new DateOnly(appointmentRequest2.StartTime.Year, appointmentRequest2.StartTime.Month, appointmentRequest2.StartTime.Day);
            mockAppointmentDL.Setup(x => x.GetAppointments(null, dateOnly)).Returns(appointments);
            var expectedGuid = Guid.NewGuid();
            mockAppointmentDL.Setup(x => x.CreateAppointment(appointmentRequest2)).Returns(expectedGuid);
            var errorDto = new CustomError() { Message = "Appoinment conflict" };
            // Act
            Assert.Throws<HttpResponseException>(() => appointmentBL.CreateAppointment(appointmentRequest2));

            // Assert
            mockAppointmentDL.Verify(x => x.GetAppointments(null,dateOnly),Times.Once);

        }

        [Fact]
        public void TestCreateAppointment_Throws_conflictError_endTime2()
        {
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);
            var appointmentRequest2 = new AppointmentRequest
            {
                Title = "New Test Appointment",
                StartTime = DateTime.Parse("2023/10/3 09:20"),
                EndTime = DateTime.Parse("2023/10/3 10:30")
            };
            var appointments = new List<Appointment>
            {
                new Appointment { Title = "Go To Gym", StartTime = DateTime.Parse("2023/10/3 10:00"), EndTime = DateTime.Parse("2023/10/3 11:00") }
            };
            DateOnly dateOnly = new DateOnly(appointmentRequest2.StartTime.Year, appointmentRequest2.StartTime.Month, appointmentRequest2.StartTime.Day);
            mockAppointmentDL.Setup(x => x.GetAppointments(null, dateOnly)).Returns(appointments);
            var expectedGuid = Guid.NewGuid();
            mockAppointmentDL.Setup(x => x.CreateAppointment(appointmentRequest2)).Returns(expectedGuid);
            var errorDto = new CustomError() { Message = "Appoinment conflict" };
            // Act
            Assert.Throws<HttpResponseException>(() => appointmentBL.CreateAppointment(appointmentRequest2));

            // Assert
            mockAppointmentDL.Verify(x => x.GetAppointments(null,dateOnly),Times.Once);

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

        [Fact]
        public void TestDeleteAppointment_checkss_NotFound_ID_DeleteMethod()
        {
            // Arrange
            var mockAppointmentDL = new Mock<IAppointmentDL>();
            var appointmentBL = new AppointmentBL(mockAppointmentDL.Object);

            var id = Guid.NewGuid();
            var appointments = new List<Appointment>() { };
            mockAppointmentDL.Setup(x => x.GetAppointments(id, null)).Returns(appointments);

            // Act
            Assert.Throws<HttpResponseException>(() => appointmentBL.DeleteAppointment(id));

            // Assert
            mockAppointmentDL.Verify(m => m.GetAppointments(It.IsAny<Guid>(), null), Times.Once);

        }

    }
}