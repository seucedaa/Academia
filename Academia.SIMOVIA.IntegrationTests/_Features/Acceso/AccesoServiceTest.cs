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
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.IntegrationTests.Data.Acceso;
using Academia.SIMOVIA.IntegrationTests.Data.General;

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
            await PoblarBaseDeDatosAsync();

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

            if (!db.Colaboradores.Any())
                db.Colaboradores.Add(ColaboradoresData.ColaboradorPrueba);

            if (!db.Usuarios.Any())
                db.Usuarios.Add(UsuariosData.UsuarioPrueba);

            if (!db.Pantallas.Any())
                db.Pantallas.Add(PantallasData.PantallaDashboard);

            if (!db.PantallasPorRoles.Any())
                db.PantallasPorRoles.Add(PantallasPorRolesData.PantallaAsignada);


            await db.SaveChangesAsync();
        }

    }
}
