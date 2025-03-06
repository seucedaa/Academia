using Academia.SIMOVIA.IntegrationTests._Features.Acceso.Data;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SIMOVIAContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var unitOfWorkDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
                if (unitOfWorkDescriptor != null)
                {
                    services.Remove(unitOfWorkDescriptor);
                }

                services.AddDbContext<SIMOVIAContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                services.AddScoped<IUnitOfWork>(serviceProvider =>
                {
                    var dbContext = serviceProvider.GetRequiredService<SIMOVIAContext>();
                    return new UnitOfWork(dbContext);
                });
            });

            builder.UseEnvironment("Testing");
        }
    }

}
