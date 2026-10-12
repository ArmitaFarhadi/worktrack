
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Contracts;
using WorkTrack.Infrastructure.Data;
using WorkTrack.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Read the connection string from configuration
var connectionString = builder.Configuration
    .GetConnectionString("WorkTrackDb")
    ?? throw new InvalidOperationException(
        "Connection string 'WorkTrackDb' was not found.");

// Register EF Core database context
builder.Services.AddDbContext<WorkTrackDbContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<
    IWorkItemStore,
    EfWorkItemStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
