using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

namespace Academia.SIMOVIA.IntegrationTests._Features.Viaje.DataTests
{
    public class ViajeTest : TheoryData<ViajeDto>
    {
        public ViajeTest()
        {
            Add(new ViajeDto
            {
                FechaHora = DateTime.Now,
                SucursalId = 1,
                TransportistaId = 1,
                UsuarioCreacionId = 1,
                Colaboradores = new List<ViajeDetallesDto>
                {
                    new ViajeDetallesDto { ColaboradorId = 1 }
                }
            });
        }
    }
}
