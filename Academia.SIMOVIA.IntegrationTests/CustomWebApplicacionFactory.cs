using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Academia.SIMOVIA.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private Mock<IUbicacionService> _mockUbicacionService;
        public Mock<IUnitOfWork>? MockUnitOfWork { get; private set; }
        private Func<Task<bool>>? _exceptionSimulation;

        public CustomWebApplicationFactory() { }

        public void ConfigureMock(Mock<IUbicacionService> mockUbicacionService)
        {
            _mockUbicacionService = mockUbicacionService;
        }

        public void ConfigureExceptionSimulation(Func<Task<bool>> exceptionAction)
        {
            _exceptionSimulation = exceptionAction;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SIMOVIAContext>));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                var unitOfWorkDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
                if (unitOfWorkDescriptor != null)
                    services.Remove(unitOfWorkDescriptor);

                var ubicacionServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUbicacionService));
                if (ubicacionServiceDescriptor != null)
                    services.Remove(ubicacionServiceDescriptor);

                services.AddDbContext<SIMOVIAContext>(options =>
                {
                    options.UseInMemoryDatabase("TestSIMOVIA")
                           .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });

                services.AddScoped<IUnitOfWork>(sp =>
                {
                    var dbContext = sp.GetRequiredService<SIMOVIAContext>();
                    var realUnitOfWork = new UnitOfWork(dbContext);
                    if (_exceptionSimulation != null)
                    {
                        MockUnitOfWork = new Mock<IUnitOfWork>();
                        MockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(_exceptionSimulation);
                        MockUnitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>())).Returns(() => realUnitOfWork.BeginTransactionAsync());
                        MockUnitOfWork.Setup(u => u.CommitAsync()).Returns(() => realUnitOfWork.CommitAsync());
                        MockUnitOfWork.Setup(u => u.RollBackAsync()).Returns(() => realUnitOfWork.RollBackAsync());

                        MockUnitOfWork.Setup(u => u.Repository<Roles>()).Returns(realUnitOfWork.Repository<Roles>());
                        MockUnitOfWork.Setup(u => u.Repository<Usuarios>()).Returns(realUnitOfWork.Repository<Usuarios>());
                        MockUnitOfWork.Setup(u => u.Repository<Cargos>()).Returns(realUnitOfWork.Repository<Cargos>());
                        MockUnitOfWork.Setup(u => u.Repository<EstadosCiviles>()).Returns(realUnitOfWork.Repository<EstadosCiviles>());
                        MockUnitOfWork.Setup(u => u.Repository<Ciudades>()).Returns(realUnitOfWork.Repository<Ciudades>());
                        MockUnitOfWork.Setup(u => u.Repository<Sucursales>()).Returns(realUnitOfWork.Repository<Sucursales>());
                        MockUnitOfWork.Setup(u => u.Repository<Colaboradores>()).Returns(realUnitOfWork.Repository<Colaboradores>());
                        MockUnitOfWork.Setup(u => u.Repository<ColaboradoresPorSucursal>()).Returns(realUnitOfWork.Repository<ColaboradoresPorSucursal>());
                        MockUnitOfWork.Setup(u => u.Repository<Transportistas>()).Returns(realUnitOfWork.Repository<Transportistas>());
                        MockUnitOfWork.Setup(u => u.Repository<ViajesEncabezado>()).Returns(realUnitOfWork.Repository<ViajesEncabezado>());
                        MockUnitOfWork.Setup(u => u.Repository<ViajesDetalle>()).Returns(realUnitOfWork.Repository<ViajesDetalle>());

                        return MockUnitOfWork.Object;
                    }
                    return realUnitOfWork;
                });

                if (_mockUbicacionService != null)
                {
                    services.AddScoped<IUbicacionService>(_ => _mockUbicacionService.Object);
                }
            });

            builder.UseEnvironment("Testing");
        }
    }
}