using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.DataSource.Variables.Value;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EmberKernel.Services.Statistic.DataSource
{
    public class EmberDataSource : IDataSource
    {
        public IEnumerable<Variable> Variables => throw new NotImplementedException();

        public event Action<IEnumerable<string>> OnMultiDataChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public Variable GetVariable(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Variable> GetVariables(IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }

        public void Publish(Variable variable)
        {
            throw new NotImplementedException();
        }

        public void Publish(string name, IValue value)
        {
            throw new NotImplementedException();
        }

        public void Register(Variable variable, IValue fallback)
        {
            throw new NotImplementedException();
        }

        public void Unregister(Variable variable)
        {
            throw new NotImplementedException();
        }
    }
}
