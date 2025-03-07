using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests.Mocks
{
    public class UnitOfWorkMock
    {
        public Mock<IUnitOfWork>? MockUnitOfWork { get; private set; }
        private readonly Func<Task<bool>>? _exceptionSimulation;

        public UnitOfWorkMock(Func<Task<bool>>? exceptionSimulation = null)
        {
            _exceptionSimulation = exceptionSimulation;
        }

        public void ConfigureMockUnitOfWork(IServiceCollection services)
        {
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
        }
    }
}
