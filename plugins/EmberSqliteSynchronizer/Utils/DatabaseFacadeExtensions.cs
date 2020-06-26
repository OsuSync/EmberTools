using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EmberSqliteSynchronizer.Utils
{
    public static class DatabaseFacadeExtensions
    {
        public static async Task<T> AutoCommitTransactionScope<T>(this DatabaseFacade facade, Func<Task<T>> scope, CancellationToken token = default)
        {
            using var transaction = await facade.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                var result = await scope().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }

        public static async Task SaveChangeAndValid(this DbContext context, int count)
        {
            if (await context.SaveChangesAsync().ConfigureAwait(false) != count)
            {
                throw new Exception("Valid save changes effect rows failed");
            }
        }

        public static IEnumerable<List<T>> Chunk<T>(this List<T> list, int size = 50)
        {
            for (int i = 0; i < list.Count; i += size) yield return list.GetRange(i, Math.Min(size, list.Count - i));
        }

        /// <summary>
        /// Update value to EntityFramework tracked entity by a new standalone entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="newEntity"></param>
        /// <param name="emitSet"></param>
        /// <returns></returns>
        public static int Update<T>(this EntityEntry<T> entry, T newEntity, HashSet<string> emitSet) where T : class
        {
            var type = typeof(T);
            var oldEntity = entry.Entity;
            var updatedProperty = 0;
            foreach (var propertyInfo in type.GetProperties())
            {
                if (!emitSet.Contains(propertyInfo.Name) && propertyInfo.CanWrite)
                {
                    var newValue = propertyInfo.GetValue(newEntity);
                    var oldValue = propertyInfo.GetValue(oldEntity);
                    if (!Equals(oldValue, newValue))
                    {
                        propertyInfo.SetValue(oldEntity, newValue);
                        updatedProperty += 1;
                    }
                }
            }
            return updatedProperty;
        }
    }
}
