using SearchService.Extensions;
using SearchService.Routes;

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


app.MapTransactionEndpoins();

app.Run();
