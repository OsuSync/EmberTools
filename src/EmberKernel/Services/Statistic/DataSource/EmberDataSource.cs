﻿using EmberKernel.Services.Statistic.DataSource.Variables;
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

        public event Action<IEnumerable<Variable>> OnMultiDataChanged;

        public bool TryGetVariable(string id, out Variable variable)
        {
            return VariableMap.TryGetValue(id, out variable);
        }

        public IEnumerable<Variable> GetVariables(IEnumerable<string> variableIds)
        {
            return variableIds.Where(VariableMap.ContainsKey).Select((name) => VariableMap[name]);
        }

        public void Publish(Variable variable)
        {
            Publish(variable.Id, variable.Value);
        }

        public void Publish(string id, IValue value)
        {
            if (VariableMap.ContainsKey(id))
            {
                var variable = VariableMap[id];
                if (!Equals(variable.Value, value))
                {
                    variable.Value = value ?? VariableFallback[id];
                    variable.OnPropertyChanged();
                    OnMultiDataChanged?.Invoke(Enumerable.Repeat(variable, 1));
                }
            }
        }

        public void Publish(IEnumerable<Variable> variables)
        {
            IEnumerable<Variable> UpdatedVariables(IEnumerable<Variable> _variables)
            {
                foreach (var incomingVariable in _variables)
                {
                    var currentVariable = VariableMap[incomingVariable.Id];
                    if (!Equals(currentVariable.Value, incomingVariable.Value))
                    {
                        currentVariable.Value = incomingVariable.Value ?? VariableFallback[incomingVariable.Id];
                        currentVariable.OnPropertyChanged();
                        yield return currentVariable;
                    }
                }
            }
            OnMultiDataChanged?.Invoke(UpdatedVariables(variables).ToList());
        }

        public void Register(Variable variable, IValue fallback)
        {

            VariableMap.Add(variable.Id, variable);
            VariableFallback.Add(variable.Id, fallback);
            Add(variable);
        }

        public void Unregister(Variable variable)
        {
            VariableMap.Remove(variable.Id);
            VariableFallback.Remove(variable.Id);
            Remove(variable);
        }

        public bool IsRegistered(Variable variable)
        {
            return VariableMap.ContainsKey(variable.Id);
        }
    }
}
