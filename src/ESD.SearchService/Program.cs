using ESD.SearchService.Extensions;
using ESD.SearchService.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllServices(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTransactionEndpoins();

app.MapHealthChecks("/health");

app.Run();
