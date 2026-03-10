using JobApplicationTracker.Api.Errors;
using JobApplicationTracker.Api.Handlers;
using JobApplicationTracker.Api.Interfaces;
using JobApplicationTracker.Api.Models.Persistence;
using JobApplicationTracker.Api.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbOptions>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddScoped<IApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IApplicationHandler, ApplicationHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
