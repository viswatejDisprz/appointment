using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using Microsoft.OpenApi.Models;
using AppointmentApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
  options.Filters.Add(new HttpResponseExceptionFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Appointment API",
    Description = "An ASP.NET Core Web API for managing Appointments in a day",
  });

});


builder.Services.AddSingleton<IAppointmentDL, AppointmentDL>();
builder.Services.AddSingleton<IAppointmentBL, AppointmentBL>();


var app = builder.Build();

app.UseRouting();

app.UseExceptionHandler();


app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }