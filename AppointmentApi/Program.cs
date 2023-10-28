using AppointmentApi.DataAccess;
using AppointmentApi.Buisness;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
  options.Filters.Add(new HttpResponseExceptionFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();
// builder.Services.AddTransient<ErrorHandlingMiddleware>();
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