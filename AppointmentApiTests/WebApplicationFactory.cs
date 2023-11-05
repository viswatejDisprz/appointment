// using System.Data.Common;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.EntityFrameworkCore;
// using AppointmentApi;
// using Microsoft.AspNetCore.Hosting;

// namespace AppointmentApi.Tests;

// // <snippet1>
// public class CustomWebApplicationFactory<TProgram>
//     : WebApplicationFactory<TProgram> where TProgram : class
// {
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {

//         builder.UseEnvironment("Development");
//     }
// }
// // </snippet1>

using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using AppointmentApi;
using Microsoft.AspNetCore.Hosting;
using AppointmentApi.Models;

namespace AppointmentApi.Tests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

    }
}
