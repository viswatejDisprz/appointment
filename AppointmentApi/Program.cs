using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddSingleton < IAppointmentDL, AppointmentDL > ();
builder.Services.AddSingleton < IAppointmentBL, AppointmentBL > ();
builder.Services.AddLogging(loggingBuilder => {
  loggingBuilder.AddConsole();
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseRouting();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment()) {
  app.UseDeveloperExceptionPage();
} else {
  app.UseHsts();
}

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();