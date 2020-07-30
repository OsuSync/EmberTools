using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.DataSource.Variables.Value;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EmberKernel.Services.Statistic
{
    public interface IDataSource : IList<Variable>, INotifyCollectionChanged, IKernelService
    {
        event Action<IEnumerable<Variable>> OnMultiDataChanged;
        IEnumerable<Variable> Variables { get; }
        bool TryGetVariable(string Id, out Variable variable);
        IEnumerable<Variable> GetVariables(IEnumerable<string> variableIds);
        bool IsRegistered(Variable variable);
        void Register(Variable variable, IValue fallback);
        void Unregister(Variable variable);
        void Publish(Variable variable);
        void Publish(string name, IValue value);
        void Publish(IEnumerable<Variable> variables);
    }
}
