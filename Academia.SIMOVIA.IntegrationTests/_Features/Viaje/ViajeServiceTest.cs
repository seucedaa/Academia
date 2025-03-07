using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Academia.SIMOVIA.WebAPI.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Academia.SIMOVIA.IntegrationTests._Features.Viaje.DataTests;
using Academia.SIMOVIA.IntegrationTests.Data.Viaje;
using Academia.SIMOVIA.IntegrationTests.Data.Acceso;
using Academia.SIMOVIA.IntegrationTests.Data.General;

namespace Academia.SIMOVIA.IntegrationTests._Features.Viaje
{
    public class ViajeServiceTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private const string BaseUrl = "/api/ViajeEncabezado/RegistrarViaje";
        private readonly Mock<IUbicacionService> _mockUbicacionService;

        public ViajeServiceTest(CustomWebApplicationFactory<Program> factory)
        {
            _mockUbicacionService = new Mock<IUbicacionService>();

            _mockUbicacionService.Setup(us => us.CalcularDistanciaViaje(
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<List<(decimal Latitud, decimal Longitud)>>()))
                .ReturnsAsync(10m);

            _customWebApplicationFactory = factory;
            _customWebApplicationFactory.ConfigureMock(_mockUbicacionService);

            _httpClient = _customWebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Dado_UnViajeValido_CuandoSeRegistra_Entonces_RetornaOkYDetalle()
        {
            await PoblarBaseDeDatosAsync();
            var viajeDto = new ViajeDto
            {
                FechaHora = DateTime.Now,
                SucursalId = 1,
                TransportistaId = 1,
                UsuarioCreacionId = 1,
                Colaboradores = new List<ViajeDetallesDto>
                {
                    new ViajeDetallesDto { ColaboradorId = 1 }
                }
            };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", viajeDto);

            _mockUbicacionService.Verify(us => us.CalcularDistanciaViaje(
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<List<(decimal, decimal)>>()), Times.AtLeastOnce());

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultado = await response.Content.ReadFromJsonAsync<Response<ViajesEncabezado>>();
            resultado.Should().NotBeNull();
            resultado!.Exitoso.Should().BeTrue();
            resultado.Mensaje.Should().Be(Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Viaje"));
        }

        [Theory]
        [ClassData(typeof(ViajeTest))]
        public async Task CuandoBaseDeDatosSeCae_EntoncesRetornaError(ViajeDto viajeDto)
        {
            await PoblarBaseDeDatosAsync();

            _customWebApplicationFactory.ConfigureExceptionSimulation(() => Task.FromException<bool>(new DbUpdateException("Simulated database failure")));

            var cliente = _customWebApplicationFactory.CreateClient();

            var response = await cliente.PostAsJsonAsync($"{BaseUrl}", viajeDto);

            _customWebApplicationFactory.MockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.AtLeastOnce());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var mensaje = await response.Content.ReadAsStringAsync(); 
            mensaje.Should().Be(Mensajes.ERROR_BASE_DE_DATOS);
        }

        [Theory]
        [ClassData(typeof(ViajeTest))]
        public async Task CuandoBaseDeDatosTardaMucho_EntoncesRetornaError(ViajeDto viajeDto)
        {
            await PoblarBaseDeDatosAsync();

            _customWebApplicationFactory.ConfigureExceptionSimulation(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10)); 
                throw new TimeoutException("Simulated database timeout");
            });

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", viajeDto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var mensaje = await response.Content.ReadAsStringAsync();
            mensaje.Should().Be(Mensajes.SERVIDOR_NO_RESPONDE);
        }


        [Theory]
        [ClassData(typeof(ViajeTest))]
        public async Task CuandoSeRegistranMultiplesViajesConcurrencia_EntoncesSeManejaCorrectamente(ViajeDto viajeDto)
        {
            await PoblarBaseDeDatosAsync();

            var tareas = Enumerable.Range(0, 5)
                .Select(_ => _httpClient.PostAsJsonAsync($"{BaseUrl}", viajeDto))
                .ToList();

            var respuestas = await Task.WhenAll(tareas);

            respuestas.Should().Contain(response => response.StatusCode == HttpStatusCode.OK);

            respuestas.Should().AllSatisfy(response =>
            {
                response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
            });

            int exitosos = respuestas.Count(r => r.StatusCode == HttpStatusCode.OK);
            int fallidos = respuestas.Count(r => r.StatusCode == HttpStatusCode.BadRequest);

        }
        
        private async Task PoblarBaseDeDatosAsync()
        {
            using var scope = _customWebApplicationFactory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<SIMOVIAContext>();

            db.Database.EnsureCreated();

            if (!db.Roles.Any())
                db.Roles.Add(RolesData.RolPrueba);

            if (!db.Cargos.Any())
                db.Cargos.Add(CargosData.CargoPrueba);

            if (!db.EstadosCiviles.Any())
                db.EstadosCiviles.Add(EstadosCivilesData.EstadoPrueba);

            if (!db.Ciudades.Any())
                db.Ciudades.Add(CiudadesData.CiudadPrueba);

            if (!db.Sucursales.Any())
                db.Sucursales.Add(SucursalesData.SucursalPrueba);

            if (!db.Usuarios.Any())
                db.Usuarios.Add(UsuariosData.UsuarioPrueba);

            if (!db.Colaboradores.Any())
                db.Colaboradores.Add(ColaboradoresData.ColaboradorPrueba);

            if (!db.ColaboradoresPorSucursal.Any())
                db.ColaboradoresPorSucursal.Add(ColaboradoresPorSucursalData.AsignacionColaboradorSucursal);

            if (!db.Transportistas.Any())
                db.Transportistas.Add(TransportistasData.TransportistaPrueba);

            await db.SaveChangesAsync();
        }

    }
}
