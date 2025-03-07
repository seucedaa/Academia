using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using FluentAssertions;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using System.Security.Cryptography;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.IntegrationTests.Data.General;
using Academia.SIMOVIA.IntegrationTests.Data.Acceso;
using Academia.SIMOVIA.IntegrationTests.Data.Viaje;

namespace Academia.SIMOVIA.IntegrationTests._Features.General
{
    public class GeneralServiceTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private const string BaseUrl = "/api/Colaborador";

        public GeneralServiceTest(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _customWebApplicationFactory = factory;
        }

        [Fact]
        public async Task Dado_UnColaboradorValido_CuandoSeRegistra_Entonces_RetornaOkYDetalle()
        {
            await PoblarBaseDeDatosAsync();

            var colaboradorDto = new ColaboradorDto
            {
                DNI = "9876543210987",
                Nombres = "Juan",
                Apellidos = "Perez",
                CorreoElectronico = "juan.perez@gmail.com",
                Telefono = "87654321",
                Sexo = "M",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                DireccionExacta = "Ciudad Central",
                Latitud = 14.25m,
                Longitud = -89.235m,
                EstadoCivilId = 1,
                CargoId = 1,
                CiudadId = 1,
                UsuarioCreacionId = 1,
                Sucursales = new List<ColaboradorPorSucursalDto>
                {
                    new ColaboradorPorSucursalDto { SucursalId = 1, DistanciaKm = 34.6m }
                }
            };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/RegistrarColaborador", colaboradorDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultado = await response.Content.ReadFromJsonAsync<Response<Colaboradores>>();
            resultado.Should().NotBeNull();
            resultado!.Exitoso.Should().BeTrue();
            resultado.Mensaje.Should().Be(Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Colaborador"));
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

            await db.SaveChangesAsync();
        }

    }
}
