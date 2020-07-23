using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.DataSource.Variables.Value;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EmberKernel.Services.Statistic
{
    public interface IDataSource : INotifyPropertyChanged
    {
        event Action<IEnumerable<string>> OnMultiDataChanged;
        IEnumerable<Variable> Variables { get; }
        Variable GetVariable(string name);
        IEnumerable<Variable> GetVariables(IEnumerable<string> names);
        void Register(Variable variable, IValue fallback);
        void Unregister(Variable variable);
        void Publish(Variable variable);
        void Publish(string name, IValue value);
    }
}
