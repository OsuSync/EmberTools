using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmberWeb.Utils
{
    public static class DatabaseFacadeExtensions
    {
        public static async Task<T> AutoCommitTransactionScope<T>(this DatabaseFacade facade, Func<Task<T>> scope, CancellationToken token = default)
        {
            using var transaction = await facade.BeginTransactionAsync(token).Async();
            try
            {
                var result = await scope().Async();
                await transaction.CommitAsync().Async();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync().Async();
                throw;
            }
        }

        public static async Task SaveChangeAndValid(this DbContext context, int count)
        {
            if (await context.SaveChangesAsync().Async() != count)
            {
                throw new Exception("Valid save changes effect rows failed");
            }
        }
    }
}
