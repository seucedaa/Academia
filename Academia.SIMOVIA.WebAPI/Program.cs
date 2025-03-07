using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Microsoft.EntityFrameworkCore;
using Farsiman.Extensions.Configuration;
using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.General;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder =>
        {
            if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Staging"))
            {
                corsBuilder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
            else
            {
                corsBuilder
                .WithOrigins("https://*.grupofarsiman.com", "https://*.grupofarsiman.io")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<SIMOVIAContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionStringFromENV("SIMOVIA_GFS")
//            ));

var isTesting = builder.Environment.IsEnvironment("Testing");
if(!isTesting)
{
    builder.Services.AddDbContext<SIMOVIAContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionStringFromENV("SIMOVIA_GFS")));

    builder.Services.AddScoped<IUnitOfWork>(serviceProvider =>
    {
        var dbContext = serviceProvider.GetRequiredService<SIMOVIAContext>();
        return new UnitOfWork(dbContext);
    });
}

builder.Services.AddTransient<AccesoService>();
builder.Services.AddTransient<AccesoDomainService>();
builder.Services.AddTransient<GeneralService>();
builder.Services.AddTransient<GeneralDomainService>();
builder.Services.AddTransient<ViajeService>();
builder.Services.AddTransient<ViajeDomainService>();
builder.Services.AddTransient<UbicacionService>();

builder.Services.AddScoped<UnitOfWorkBuilder>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
public partial class Program { }
