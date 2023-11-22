using AppointmentApi.Buisness;
using MockAppointmentApiTests;

namespace AppointmentApiTests
{
    public class AppointmentDLTests
    {
        private AppointmentDL appointmentDL;

        private MockAppointments mock;

        public AppointmentDLTests()
        {
            appointmentDL = new AppointmentDL();
            mock = new MockAppointments();
        }

        /// <summary>
        /// Get Appointments code
        /// </summary>
        [Fact]
        public void GetAppointments_ValidDate_ReturnsMatchingAppointments()
        {

            // Act
            var result = appointmentDL.GetAppointments(new DateOnly(2023, 11, 29));

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Create Appointment Testing
        /// </summary>
        [Fact]
        public void CreateAppointment_Validate_Returns_HigherCount()
        {
            // Act
            var initialCount = appointmentDL.GetAppointments(new DateOnly(2023, 11, 30));
            var appointmentRequest = mock.aptRequest();
            appointmentDL.CreateAppointment(appointmentRequest);
            var postCount = appointmentDL.GetAppointments(new DateOnly(2023, 11, 30));

            // Assert
            Assert.Equal(initialCount.Count + 1, postCount.Count);
        }

        /// <summary>
        /// Delete Appointment Testing
        /// </summary>
        [Fact]
        public void DeleteAppointment_Validate_Returns_LowerCount()
        {
            // Act
            var appointmentRequest = mock.aptRequest();
            var id = appointmentDL.CreateAppointment(appointmentRequest);
            var initialCount = appointmentDL.GetAppointments(new DateOnly(2023, 11, 30));
            appointmentDL.DeleteAppointment(id);
            var postCount = appointmentDL.GetAppointments(new DateOnly(2023, 11, 30));

            // Assert
            Assert.Equal(initialCount.Count - 1, postCount.Count);
        }
    }
}
