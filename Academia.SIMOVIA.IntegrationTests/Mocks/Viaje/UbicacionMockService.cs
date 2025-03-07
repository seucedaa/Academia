using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests.Mocks.Viaje
{
    public class UbicacionMockService
    {
        public Mock<IUbicacionService> Service { get; set; } = new Mock<IUbicacionService>();

        public UbicacionMockService() { }
    }

}
