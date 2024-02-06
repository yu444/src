using TimesheetApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/api/timesheets", () =>
{
    TimesheetsController controller = new();
    return controller.GetTimesheets();
})
.WithName("GetTimesheets")
.WithOpenApi();

app.MapGet("/api/timesheets/{id}", (int id) =>
{
    TimesheetsController controller = new();
    return controller.GetTimesheetById(id);
})
.WithName("GetTimesheetById")
.WithOpenApi();

app.MapPost("/api/timesheets", (TimesheetEntry newEntry) =>
{
    TimesheetsController controller = new();
    return controller.AddTimesheet(newEntry);
})
.WithName("AddTimesheet")
.WithOpenApi();




app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
