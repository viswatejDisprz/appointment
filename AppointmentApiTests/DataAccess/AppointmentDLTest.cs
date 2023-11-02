using AppointmentApi.Buisness;
using AppointmentApi.Models;

namespace AppointmentApiTests
{
    public class AppointmentDLTests
    {
        [Fact]
        public void GetAppointments_ValidDate_ReturnsMatchingAppointments()
        {
            // Arrange
            var appointmentDL = new AppointmentDL();

            // Act
            var result = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

            // Assert
            Assert.Equal(3, result.Count);
            foreach (var appointment in result)
            {
                Assert.Contains(result, app => app.StartTime == appointment.StartTime && app.EndTime == appointment.EndTime && app.Title == appointment.Title);
            }
        }

        [Fact]
        public void CreateAppointment_Validate_Returns_HigherCount()
        {
            var appointmentDL = new AppointmentDL();
            
            // Act
            var initialCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));
            var appointmentRequest = new AppointmentRequest(){StartTime=DateTime.Parse("2023/10/15 11:00"),EndTime=DateTime.Parse("2023/10/15 12:00"),Title="Walking"};
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var PostCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

            // Assert
            Assert.IsType<Guid>(id);
            Assert.Equal(initialCount.Count + 1 ,PostCount.Count);
        }

        [Fact]
        public void DeleteAppointment_Validate_Returns_LowerCount()
        {
            var appointmentDL = new AppointmentDL();
            
            // Act
            var appointmentRequest = new AppointmentRequest(){StartTime=DateTime.Parse("2023/10/15 11:00"),EndTime=DateTime.Parse("2023/10/15 12:00"),Title="Walking"};
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var initialCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));
            appointmentDL.DeleteAppointment(id);
            var PostCount = appointmentDL.GetAppointments(null, new DateOnly(2023, 10, 15));

            // Assert
            Assert.IsType<Guid>(id);
            Assert.Equal(initialCount.Count - 1 ,PostCount.Count);
        }
    }
}
