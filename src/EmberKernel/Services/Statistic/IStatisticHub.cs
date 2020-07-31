using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.Format;
using EmberKernel.Services.Statistic.Hub;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace EmberKernel.Services.Statistic
{
    public interface IStatisticHub: IFormatContainer, INotifyCollectionChanged, INotifyPropertyChanged, IKernelService, IList<HubFormat>
    {
        event Action<string, string> OnFormatUpdated;
        void Register(string name, string format);
        void Unregister(string name);
        void Update(string name, string format);
        string GetValue(string name);
        string Format(string format);
        IEnumerable<Variable> Variables { get; }

    }
}
