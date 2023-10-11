using Microsoft.EntityFrameworkCore;
using AppointmentApi.Models;
using AppointmentApi.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppointmentContext>(opt =>
    opt.UseInMemoryDatabase("AppointmentList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAppointmentRepository, InMemAppointmentRepository>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Use custom error handling for non-development environments
        app.UseExceptionHandler("/appointment/Error");
        app.UseHsts();
    }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
