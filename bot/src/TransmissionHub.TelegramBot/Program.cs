using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddHttpLogging(_ => { });
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();

app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
});

app.UseHttpLogging();

app.MapPost("/update", (HttpContext context) => Task.CompletedTask);

app.Run();