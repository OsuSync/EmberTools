using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.DataSource.Variables.Value;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmberKernel.Services.Statistic.DataSource
{
    public class EmberDataSource : ObservableCollection<Variable>, IDataSource
    {
        private readonly Dictionary<string, Variable> VariableMap = new Dictionary<string, Variable>();
        private readonly Dictionary<string, IValue> VariableFallback = new Dictionary<string, IValue>();
        public IEnumerable<Variable> Variables => VariableMap.Values;

        public event Action<IEnumerable<string>> OnMultiDataChanged;

        public bool TryGetVariable(string name, out Variable variable)
        {
            return VariableMap.TryGetValue(name, out variable);
        }

        public IEnumerable<Variable> GetVariables(IEnumerable<string> variableIds)
        {
            return variableIds.Where(VariableMap.ContainsKey).Select((name) => VariableMap[name]);
        }

        public void Publish(Variable variable)
        {
            Publish(variable.Id, variable.Value);
        }

        public void Publish(string name, IValue value)
        {
            if (VariableMap.ContainsKey(name))
            {
                var variable = VariableMap[name];
                if (!Equals(variable.Value, value))
                {
                    variable.Value = value;
                    OnMultiDataChanged?.Invoke(Enumerable.Repeat(name, 1));
                }
            }
        }

        public void Publish(IEnumerable<Variable> variables)
        {
            IEnumerable<string> UpdatedVariables()
            {
                foreach (var incomingVariable in variables)
                {
                    var currentVariable = VariableMap[incomingVariable.Id];
                    if (!Equals(currentVariable.Value, incomingVariable.Value))
                    {
                        currentVariable.Value = incomingVariable.Value;
                        yield return currentVariable.Id;
                    }
                }
            }
            OnMultiDataChanged?.Invoke(UpdatedVariables());
        }

        public void Register(Variable variable, IValue fallback)
        {

            VariableMap.Add(variable.Id, variable);
            VariableFallback.Add(variable.Id, fallback);
        }

        public void Unregister(Variable variable)
        {
            VariableMap.Remove(variable.Id);
            VariableFallback.Remove(variable.Id);
        }

        public bool IsRegistered(Variable variable)
        {
            return VariableMap.ContainsKey(variable.Id);
        }
    }
}
