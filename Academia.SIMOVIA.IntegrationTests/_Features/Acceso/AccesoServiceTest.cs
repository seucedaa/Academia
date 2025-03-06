using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using Academia.SIMOVIA.WebAPI.Helpers;
using System.Security.Cryptography;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using FluentAssertions;

namespace Academia.SIMOVIA.IntegrationTests._Features.Acceso
{
    public class AccesoServiceTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private const string baseUrl = "/api/Usuario"; 
        public AccesoServiceTest(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            _customWebApplicationFactory = factory;
        }

        [Fact]
        public async Task Dado_QueExisteElUsuario_CuandoSeInvocaElEndpointDeInicioSesion_Entonces_RetornaOkYLosDatosDeSesion()
        {
            using (var scope = _customWebApplicationFactory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SIMOVIAContext>();

                db.Database.EnsureCreated();
                db.Roles.Add(new Roles
                {
                    RolId = 1,
                    Descripcion = "prueba",
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now
                });
                byte[] claveCifrada;
                using (SHA512 sha512 = SHA512.Create())
                    claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes("sua"));
                db.Cargos.Add(new Cargos
                {
                    CargoId = 1,
                    Descripcion = "prueba",
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now
                });
                
                db.Colaboradores.Add(new Colaboradores
                {
                    ColaboradorId = 1,
                    DNI = "1234567890123",
                    Nombres = "sua",
                    Apellidos = "sua",
                    CorreoElectronico = "sua@gmail.com",
                    Telefono = "12345678",
                    Sexo = "M",
                    FechaNacimiento = DateTime.Now,
                    DireccionExacta = "los angeles",
                    Latitud = 15.25m,
                    Longitud = -88.235m,
                    EstadoCivilId = 1,
                    CargoId = 1,
                    CiudadId = 1,
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now,
                });
                db.Usuarios.Add(new Usuarios
                {
                    UsuarioId = 1,
                    Usuario = "sua",
                    Clave = claveCifrada,
                    EsAdministrador = false,
                    ColaboradorId = 1,
                    RolId = 1,
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now,
                });
                db.Pantallas.Add(new Pantallas
                {
                    PantallaId = 1,
                    Descripcion = "Dashboard",
                    DireccionURL = "url",
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now
                });

                db.PantallasPorRoles.Add(new PantallasPorRoles
                {
                    RolId = 1,
                    PantallaId = 1
                });

                db.SaveChanges();

            }

            var loginDto = new InicioSesionDto
            {
                Usuario = "sua",
                Clave = "sua" 
            };

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/InicioSesion", loginDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultado = await response.Content.ReadFromJsonAsync<Response<SesionUsuarioDto>>();
            resultado.Should().NotBeNull();
            resultado!.Exitoso.Should().BeTrue(); 
            resultado.Mensaje.Should().Be(Mensajes.SESION_EXITOSA);

            resultado.Should().BeEquivalentTo(new
            {
                Exitoso = true,
                Mensaje = Mensajes.SESION_EXITOSA
            });

        }

    }
}
