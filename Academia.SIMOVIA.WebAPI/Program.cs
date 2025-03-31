using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.General;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Extensions.Configuration;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI
{
    public partial class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            var isTesting = builder.Environment.IsEnvironment("Testing");
            if (!isTesting)
            {
                builder.Services.AddDbContext<SimoviaContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionStringFromENV("SIMOVIA_GFS")));

                builder.Services.AddScoped<IUnitOfWork>(serviceProvider =>
                {
                    var dbContext = serviceProvider.GetRequiredService<SimoviaContext>();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}