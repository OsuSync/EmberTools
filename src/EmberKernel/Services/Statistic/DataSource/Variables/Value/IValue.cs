using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.DataSource.Variables.Value
{
    public interface IValue
    {
        ValueType Type { get; }
        string ToString();
    }

    public interface IValue<T> : IValue, IEqualityComparer<IValue<T>>
    {
        T Value { get; set; }
    }
}
