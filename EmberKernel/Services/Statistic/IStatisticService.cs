using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.Property;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EmberKernel.Services.Statistic
{
    public interface IStatisticService
    {
        /// <summary>
        /// Configure a registered event to statistic services
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        void ConfigureStatistic<TEvent>() where TEvent : Event<TEvent>;
        /// <summary>
        /// Get cached category statistic
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        TEvent Current<TEvent>() where TEvent : Event<TEvent>;
        /// <summary>
        /// Get cached property statistic
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        TValue Current<TEvent, TValue>(Expression<Func<TEvent, TValue>> property) where TEvent : Event<TEvent>;
        /// <summary>
        /// Get all available property
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProperty> GetAllProperty();
        /// <summary>
        /// Get one property statistic
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IProperty Current(string property);
    }
}
