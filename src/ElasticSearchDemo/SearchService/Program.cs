using SearchService.Extensions;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/sync-data", async (ICsvService csvService) =>
{
    await csvService.ReadTransactionAsync();

    return Results.Ok();
});

app.MapGet("/weatherforecast", () =>
{
    return Results.Ok();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
