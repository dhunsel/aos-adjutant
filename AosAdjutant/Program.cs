using AosAdjutant.Database;
using AosAdjutant.Features.Factions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddHttpLogging(opts => { });

builder.Services.AddScoped<FactionService, FactionService>();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration["AosAdjutant:DbContextConnectionString"]));


var app = builder.Build();

app.UseHttpLogging();

app.UseExceptionHandler();

app.MapControllers();

app.Run();