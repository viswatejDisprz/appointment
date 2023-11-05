using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Xml;
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

  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlDocFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
  var xmlDoc = new XmlDocument();
  xmlDoc.Load(xmlDocFilePath);
  var xmlComments = xmlDoc.InnerXml;
  var replacedXmlComments = Extensions.ReplaceDynamicDateFormat(xmlComments);

  xmlDoc.LoadXml(replacedXmlComments);
  xmlDoc.Save(xmlDocFilePath);

  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddSingleton<IAppointmentDL, AppointmentDL>();
builder.Services.AddSingleton<IAppointmentBL, AppointmentBL>();
builder.Services.AddLogging(loggingBuilder =>
{
  loggingBuilder.AddConsole();
});

var app = builder.Build();

app.UseRouting();

app.UseExceptionHandler();
app.UseStatusCodePages();


  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }