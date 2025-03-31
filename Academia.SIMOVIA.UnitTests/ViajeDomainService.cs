using Academia.SIMOVIA.UnitTests.DataTests.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements.Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using FluentAssertions;

namespace Academia.SIMOVIA.UnitTests
{
    public class ViajeDomainServiceTest
    {
        private readonly ViajeDomainService _viajeDomainService;

        public ViajeDomainServiceTest()
        {
            _viajeDomainService = new ViajeDomainService();
        }

        [Theory]
        [ClassData(typeof(ViajeTest))]
        public void Dado_Un_Viaje_Cuando_Se_Valida_Para_Registro_DebeRetornarElResultadoEsperado(
            ViajesEncabezado viaje, RegistroViajeDomainRequirement domainRequirement, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _viajeDomainService.ValidarViajeParaRegistro(viaje, domainRequirement);

            resultado.Mensaje ??= "";

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje ?? "" });
        }

        [Theory]
        [ClassData(typeof(SucursalTest))]
        public void Dado_Una_Sucursal_Cuando_Se_Valida_Para_Registro_DebeRetornarElResultadoEsperado(
            Sucursales sucursal, RegistroSucursalDomainRequirement domainRequirement, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _viajeDomainService.ValidarSucursalParaRegistro(sucursal, domainRequirement);

            resultado.Mensaje ??= "";

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje ?? "" });
        }

        [Theory]
        [ClassData(typeof(DistanciaTest))]
        public void Dado_Una_Distancia_Cuando_Se_Valida_DebeRetornarElResultadoEsperado(decimal distanciaCantidad, decimal distanciaTotalKm, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _viajeDomainService.ValidarDistancia(distanciaCantidad, distanciaTotalKm);

            resultado.Mensaje ??= "";

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje ?? "" });
        }

        [Theory]
        [ClassData(typeof(UbicacionTest))]
        public void Dado_Una_Ubicacion_Cuando_Se_Valida_DebeRetornarElResultadoEsperado(decimal latitud, decimal longitud, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _viajeDomainService.ValidarUbicacion(latitud, longitud);
            resultado.Mensaje ??= "";

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje ?? "" });
        }


    }
}
