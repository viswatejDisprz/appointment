using Microsoft.EntityFrameworkCore;
using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using Microsoft.OpenApi.Models;
using YamlDotNet.Serialization;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IAppointmentDL, AppointmentDL>();
builder.Services.AddSingleton<IAppointmentBL, AppointmentBL>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

var app = builder.Build();

var provider = new FileExtensionContentTypeProvider();

// Add new mappings

// Allow YAML files to be served via HTTP request.
provider.Mappings[".yaml"] = "text/yaml"; 

app.UseStaticFiles(new StaticFileOptions
{
  ContentTypeProvider = provider
});

if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Use custom error handling for non-development environments
        // app.UseExceptionHandler("/appointment/Error");
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
