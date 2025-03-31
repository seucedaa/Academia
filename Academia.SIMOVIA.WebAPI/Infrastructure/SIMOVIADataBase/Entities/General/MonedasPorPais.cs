using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    [ExcludeFromCodeCoverage]
    public class MonedasPorPais
    {
        public MonedasPorPais()
        {
            Pais = new Paises();
            Moneda = new Monedas();
        }
        public int MonedaPorPaisId { get; set; }
        public int PaisId { get; set; }
        public int MonedaId { get; set; }
        public bool Principal { get; set; }

        public virtual Paises Pais { get; set; }
        public virtual Monedas Moneda { get; set; }
    }
}
