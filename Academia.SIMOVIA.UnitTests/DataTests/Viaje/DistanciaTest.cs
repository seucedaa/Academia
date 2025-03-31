using Academia.SIMOVIA.WebAPI.Utilities;

namespace Academia.SIMOVIA.UnitTests.DataTests.Viaje
{
    public class DistanciaTest : TheoryData<decimal, decimal, bool, string>
    {
        public DistanciaTest()
        {
            Add(-5, 10, false, Mensajes.ERROR_DISTANCIA);
            Add(50, 60, false, Mensajes.DISTANCIA_EXCEDIDA.Replace("@distanciakm", "110"));
            Add(30, 50, true, "");
        }
    }
}
