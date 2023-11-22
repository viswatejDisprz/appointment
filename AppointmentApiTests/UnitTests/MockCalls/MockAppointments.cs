using Moq;
using AppointmentApi.Buisness;
using AppointmentApi.Models;
using AppointmentApi.DataAccess;
using Microsoft.IdentityModel.Tokens;

namespace MockAppointmentApiTests
{
    public class MockAppointments
    {


        public List<Appointment> appointments => new List<Appointment>
        {
            new Appointment { 
                Title = "Go To Gym",
                StartTime = DateTime.Parse("2023/11/30 10:00"),
                EndTime = DateTime.Parse("2023/11/30 10:30") }
        };


        public AppointmentDateRequest aptDateRequest() =>
           new AppointmentDateRequest() { Date = new DateOnly(2023, 11, 30) };

        public AppointmentRequest aptRequest(string? startTime = null, string? endTime = null) =>
             new AppointmentRequest
             {
                 Title = "New Test Appointment",
                 StartTime = DateTime.Parse(!startTime.IsNullOrEmpty() ? $"2023/11/30 {startTime}" : "2023/11/30 11:00"),
                 EndTime = DateTime.Parse(!endTime.IsNullOrEmpty() ? $"2023/11/30 {endTime}" : "2023/11/30 12:00")
             };


        public void GetAppointmentMock(Mock<IAppointmentBL>? mockAppointmentBL = null, Mock<IAppointmentDL>? mockAppointmentDL = null, AppointmentDateRequest? appointmentDateRequest = null)
        {
            if (mockAppointmentBL is null)
                mockAppointmentDL.Setup(x => x.GetAppointments(aptDateRequest().Date)).Returns(appointments);
            else
                mockAppointmentBL.Setup(x => x.GetAppointments(appointmentDateRequest)).Returns(appointments);
        }

        public CustomError customError() =>
          new CustomError
          {
              Message = "This is a sample error message."
          };

    }
};