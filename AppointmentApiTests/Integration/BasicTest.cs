// using AppointmentApi.Buisness;
// using AppointmentApi.Models;
// using Microsoft.AspNetCore.Mvc.Testing;
// using AppointmentApi.Tests;
// using AppointmentApi.Controllers;
// using AppointmentApi.DataAccess;

// public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//     private readonly CustomWebApplicationFactory<Program> _factory;

//     public BasicTests(CustomWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//         _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
//         {
//             AllowAutoRedirect = false
//         });
//     }

//     [Fact]
//     public void Post_DeleteAllMessagesHandler_ReturnsRedirectToRoot()
//     {
//         // Arrange
//         var client = _factory.CreateClient();

//         // Act
//         var appointmentDateRequest = new AppointmentDateRequest { Date = DateOnly.Parse("2023-10-15") };

//         // Act
//         var response = client;

//         // Assert
//         Assert.NotEmpty(response);
//     }
// }

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using AppointmentApi.Models;
using AppointmentApi.Controllers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace AppointmentApi.Tests
{
    public class AppointmentControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AppointmentControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAppointments_ReturnsSuccessStatusCode()
        {
            var appointmentDateRequest = new AppointmentDateRequest { Date = new DateOnly(2023, 10, 15) };
            var request = $"/v1/appointments?Date={appointmentDateRequest.Date}";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateAppointment_ReturnsSuccessStatusCode()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Title = "Sample Appointment"
            };

            var content = new StringContent(JsonConvert.SerializeObject(appointmentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/appointments", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAppointment_ReturnsSuccessStatusCode()
        {

            var appointmentId = Guid.NewGuid();
            var request = $"/v1/appointments/{appointmentId}";

            // Act
            var response = await _client.DeleteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
