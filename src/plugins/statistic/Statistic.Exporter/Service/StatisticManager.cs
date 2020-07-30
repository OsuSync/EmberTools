using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.Statistic;
using EmberStatisticDatabase.Database;
using EmberStatisticDatabase.Model;
using Statistic.Abstract.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Statistic.Exporter.Service
{
    public class StatisticManager : IComponent, IEventHandler<FormatCreatedEvent>
    {
        public StatisticContext Db { get; set; }
        public IStatisticHub Hub { get; set; }


        private readonly Dictionary<string, RegisteredFormat> DatabaseFormats = new Dictionary<string, RegisteredFormat>();
        public async ValueTask InitializeRegisteredFormat()
        {
            await foreach (var item in Db.RegisteredFormats.AsAsyncEnumerable())
            {
                Hub.Register(item.Name, item.Format);
                DatabaseFormats.Add(item.Name, item);
            };
        }

        public ValueTask UninitializeRegisteredFormat()
        {
            foreach (var (name, _) in DatabaseFormats)
            {
                Hub.Unregister(name);
            }
            DatabaseFormats.Clear();
            return default;
        }

        public void Dispose() {}

        public async ValueTask Handle(FormatCreatedEvent @event)
        {
            var trx = await Db.Database.BeginTransactionAsync();
            try
            {
                // update existed format
                if (DatabaseFormats.ContainsKey(@event.Name))
                {
                    var existFormat = DatabaseFormats[@event.Name];
                    existFormat.Format = @event.Format;
                    existFormat.UpdatedAt = DateTime.Now;
                    Db.RegisteredFormats.Update(existFormat);
                    await Db.SaveChangesAsync();
                }
                // persist new format
                else
                {
                    var entity = (await Db.RegisteredFormats.AddAsync(new RegisteredFormat()
                    {
                        Name = @event.Name,
                        Format = @event.Format,
                        LastValue = string.Empty,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    })).Entity;
                    DatabaseFormats.Add(entity.Name, entity);
                    await Db.SaveChangesAsync();
                }
                await trx.CommitAsync();
            }
            catch
            {
                await trx.RollbackAsync();
            }
            
        }
    }
}
