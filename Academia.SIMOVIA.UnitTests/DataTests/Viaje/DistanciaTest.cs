using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.UnitTests.DataTests.Viaje
{
    public class DistanciaTest : TheoryData<decimal, decimal, bool, string>
    {
        public DistanciaTest()
        {
            Add(-5, 10, false, Mensajes.ERROR_DISTANCIA);
            Add(50, 60, false, Mensajes.DISTANCIA_EXCEDIDA.Replace("@distanciakm", "110"));
            Add(30, 50, true, null);
        }
    }
}
