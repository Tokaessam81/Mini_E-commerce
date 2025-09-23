using E_commerce.PL.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApplicationPipeline();
app.Run();
