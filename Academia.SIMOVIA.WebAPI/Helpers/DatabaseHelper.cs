using Academia.SIMOVIA.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Academia.SIMOVIA.WebAPI.Helpers
{
    public static class DatabaseHelper
    {
        public static async Task<bool> ExisteRegistroEnBD<TEntity>(UnitOfWorkBuilder unitOfWorkBuilder, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var unitOfWork = unitOfWorkBuilder.BuildDbSIMOVIA();
            return await unitOfWork.Repository<TEntity>().AsQueryable().AnyAsync(predicate);
        }
    }
}
