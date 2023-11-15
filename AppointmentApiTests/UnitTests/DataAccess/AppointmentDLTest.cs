using AppointmentApi.Buisness;
using AppointmentApi.Models;

namespace AppointmentApiTests
{
    public class AppointmentDLTests
    {
        private AppointmentDL appointmentDL;

        public AppointmentDLTests()
        {
            appointmentDL = new AppointmentDL();
        }

        [Fact]
        public void GetAppointments_ValidDate_ReturnsMatchingAppointments()
        {
            // Arrange
            var date = new DateOnly(2023, 11, 30);
            var date2 = new DateOnly(2023, 11, 29);
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023/11/30 11:00"), EndTime = DateTime.Parse("2023/11/30 12:00"), Title = "Walking" };

            // Act
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var result3 = appointmentDL.GetAppointments(date: date2);
            // Assert
            Assert.Empty(result3);
        }

        [Fact]
        public void CreateAppointment_Validate_Returns_HigherCount()
        {
            // Act
            var initialCount = appointmentDL.GetAppointments(new DateOnly(2023, 10, 15));
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023/10/15 11:00"), EndTime = DateTime.Parse("2023/10/15 12:00"), Title = "Walking" };
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var postCount = appointmentDL.GetAppointments(new DateOnly(2023, 10, 15));

            // Assert
            Assert.IsType<Guid>(id);
            Assert.Equal(initialCount.Count + 1, postCount.Count);
        }

        [Fact]
        public void DeleteAppointment_Validate_Returns_LowerCount()
        {
            // Act
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023/10/15 11:00"), EndTime = DateTime.Parse("2023/10/15 12:00"), Title = "Walking" };
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var initialCount = appointmentDL.GetAppointments(new DateOnly(2023, 10, 15));
            appointmentDL.DeleteAppointment(id);
            var postCount = appointmentDL.GetAppointments(new DateOnly(2023, 10, 15));

            // Assert
            Assert.IsType<Guid>(id);
            Assert.Equal(initialCount.Count - 1, postCount.Count);
        }
    }
}
