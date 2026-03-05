using AosAdjutant.Api.Database;
using AosAdjutant.Api.Features.Factions;
using AosAdjutant.Api.Features.Factions.BattleFormations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddHttpLogging(opts => { });

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration["AosAdjutant:DbContextConnectionString"]));

var app = builder.Build();

app.UseHttpLogging();

app.UseExceptionHandler();

app.MapControllers();

app.Run();