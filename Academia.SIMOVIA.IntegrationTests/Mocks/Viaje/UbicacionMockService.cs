using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Moq;

namespace Academia.SIMOVIA.IntegrationTests.Mocks.Viaje
{
    public class UbicacionMockService
    {
        public Mock<IUbicacionService> Service { get; set; } = new Mock<IUbicacionService>();

        public UbicacionMockService() { }
    }

}
