using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.Statistic;
using EmberStatisticDatabase.Database;
using EmberStatisticDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Statistic.Abstract.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Statistic.Exporter.Service
{
    public class StatisticManager : IComponent,
        IEventHandler<FormatCreatedEvent>,
        IEventHandler<FormatUpdatedEvent>,
        IEventHandler<FormatDeletedEvent>
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

        public async ValueTask CreateOrUpdateFormat(string name, string format, string newName = null)
        {
            var trx = await Db.Database.BeginTransactionAsync();
            try
            {
                // update existed format
                if (DatabaseFormats.ContainsKey(name))
                {
                    var existFormat = DatabaseFormats[name];
                    if (newName != null)
                    {
                        existFormat.Name = newName;
                    }
                    existFormat.Format = format;
                    existFormat.UpdatedAt = DateTime.Now;
                    Db.RegisteredFormats.Update(existFormat);
                    await Db.SaveChangesAsync();
                }
                // persist new format
                else
                {
                    var entity = (await Db.RegisteredFormats.AddAsync(new RegisteredFormat()
                    {
                        Name = name,
                        Format = format,
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

        public async ValueTask DeleteFormatIfExist(string name)
        {

            var trx = await Db.Database.BeginTransactionAsync();
            try
            {
                var format = await Db.RegisteredFormats.SingleOrDefaultAsync(f => f.Name == name);
                Db.RegisteredFormats.Remove(format);
                await Db.SaveChangesAsync();
            }
            catch
            {
                await trx.RollbackAsync();
            }
        }

        public ValueTask Handle(FormatCreatedEvent @event)
        {
            return CreateOrUpdateFormat(@event.Name, @event.Format);
        }

        public ValueTask Handle(FormatUpdatedEvent @event)
        {
            return CreateOrUpdateFormat(@event.Name, @event.Format, @event.NewName);
        }

        public ValueTask Handle(FormatDeletedEvent @event)
        {
            return DeleteFormatIfExist(@event.Name);
        }
    }
}
