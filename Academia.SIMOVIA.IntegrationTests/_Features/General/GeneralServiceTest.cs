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
            await SeedDatabaseAsync();

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

        private async Task SeedDatabaseAsync()
        {
            using var scope = _customWebApplicationFactory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<SIMOVIAContext>();

            db.Database.EnsureCreated();

            if (!db.Roles.Any())
                db.Roles.Add(new Roles { RolId = 1, Descripcion = "Admin", UsuarioCreacionId = 1, FechaCreacion = DateTime.Now });

            if (!db.Cargos.Any())
                db.Cargos.Add(new Cargos { CargoId = 1, Descripcion = "Gerente", UsuarioCreacionId = 1, FechaCreacion = DateTime.Now });

            if (!db.EstadosCiviles.Any())
                db.EstadosCiviles.Add(new EstadosCiviles { EstadoCivilId = 1, Descripcion = "Soltero", UsuarioCreacionId = 1, FechaCreacion = DateTime.Now });

            if (!db.Ciudades.Any())
                db.Ciudades.Add(new Ciudades { CiudadId = 1, Descripcion = "Ciudad Central", UsuarioCreacionId = 1, FechaCreacion = DateTime.Now });

            if (!db.Sucursales.Any())
                db.Sucursales.Add(new Sucursales { SucursalId = 1, Descripcion = "Sucursal 1", UsuarioCreacionId = 1, FechaCreacion = DateTime.Now });

            if (!db.Usuarios.Any())
            {
                byte[] claveCifrada;
                using (var sha512 = SHA512.Create())
                    claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes("admin123"));

                db.Usuarios.Add(new Usuarios
                {
                    UsuarioId = 1,
                    Usuario = "admin",
                    Clave = claveCifrada,
                    EsAdministrador = true,
                    ColaboradorId = 1,
                    RolId = 1,
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now,
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
