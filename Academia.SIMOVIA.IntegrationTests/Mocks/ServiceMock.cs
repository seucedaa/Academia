using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Academia.SIMOVIA.IntegrationTests._Features.Acceso.Data;

namespace Academia.SIMOVIA.IntegrationTests.Mocks
{
    public static class ServiceMock
    {
        public static void SetupDatabaseDown(IUnitOfWork unitOfWork)
        {
            var usuariosRepo = Substitute.For<IRepository<Usuarios>>();
            usuariosRepo.AsQueryable().Returns(_ => throw new Exception("Simulación de base de datos caída"));
            unitOfWork.Repository<Usuarios>().Returns(usuariosRepo);
        }

        public static void SetupTimeout(IUnitOfWork unitOfWork)
        {
            var usuariosRepo = Substitute.For<IRepository<Usuarios>>();
            usuariosRepo.AsQueryable().Returns(_ =>
            {
                Task.Delay(31000).Wait();
                return new List<Usuarios> { InicioSesionData.ObtenerUsuario("sua") }.AsQueryable();
            });
            unitOfWork.Repository<Usuarios>().Returns(usuariosRepo);
        }

        public static void SetupUnhandledException(IUnitOfWork unitOfWork)
        {
            var usuariosRepo = Substitute.For<IRepository<Usuarios>>();
            usuariosRepo.AsQueryable().Returns(_ => throw new Exception("Ocurrió un error inesperado"));
            unitOfWork.Repository<Usuarios>().Returns(usuariosRepo);
        }
    }
}
