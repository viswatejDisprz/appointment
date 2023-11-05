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
            var date = new DateOnly(2023, 10, 03);
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023/10/03 11:00"), EndTime = DateTime.Parse("2023/10/03 12:00"), Title = "Walking" };

            // Act
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var result = appointmentDL.GetAppointments(id: id, null);
            var result6 = appointmentDL.GetAppointments(id: id);
            var result2 = appointmentDL.GetAppointments(null, null);
            var result4 = appointmentDL.GetAppointments();
            var result3 = appointmentDL.GetAppointments(null, date);
            var result5 = appointmentDL.GetAppointments(id, new DateOnly(2023, 10, 15));

            // Assert
            Assert.Single(result);
            Assert.Empty(result2);
            Assert.Single(result3);
            Assert.Empty(result4);
            Assert.NotEmpty(result5);
            Assert.NotEmpty(result6);
        }


        [Fact]
        public void CreateAppointment_Validate_Returns_HigherCount()
        {
            // Act
            var initialCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));
            var appointmentRequest = new AppointmentRequest { StartTime = DateTime.Parse("2023/10/15 11:00"), EndTime = DateTime.Parse("2023/10/15 12:00"), Title = "Walking" };
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var postCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

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
            var initialCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));
            appointmentDL.DeleteAppointment(id);
            var postCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

            // Assert
            Assert.IsType<Guid>(id);
            Assert.Equal(initialCount.Count - 1, postCount.Count);
        }
    }
}
