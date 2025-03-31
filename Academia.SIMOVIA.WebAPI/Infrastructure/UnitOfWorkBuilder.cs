using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;

namespace Academia.SIMOVIA.WebAPI.Infrastructure
{
    public class UnitOfWorkBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork BuildDbSIMOVIA()
        {
            var dbContext = _serviceProvider.GetRequiredService<SimoviaContext>();
            return new UnitOfWork(dbContext);
        }
    }
}
