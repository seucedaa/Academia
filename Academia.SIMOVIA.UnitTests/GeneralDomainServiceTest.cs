using Academia.SIMOVIA.UnitTests.DataTests.General;
using Academia.SIMOVIA.WebAPI._Features.General;
using Academia.SIMOVIA.WebAPI._Features.General.DomainRequirements;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using FluentAssertions;

namespace Academia.SIMOVIA.UnitTests
{
    public class GeneralDomainServiceTest
    {
        private readonly GeneralDomainService _generalDomainService;

        public GeneralDomainServiceTest()
        {
            _generalDomainService = new GeneralDomainService();
        }

        [Theory]
        [ClassData(typeof(ColaboradorTest))]
        public void Dado_Un_Colaborador_Cuando_Se_Valida_Para_Registro_DebeRetornarElResultadoEsperado(
            Colaboradores colaborador, RegistroColaboradorDomainRequirement domainRequirement, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _generalDomainService.ValidarColaboradorParaRegistro(colaborador, domainRequirement);

            resultado.Mensaje ??= "";

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje ?? "" });

        }

    }
}
