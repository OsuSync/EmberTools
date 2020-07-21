namespace EmberKernel.Services.Statistic.DataSource.Variables.Value
{
    public interface IValue
    {
        ValueType Type { get; }
    }

    public interface IValue<T> : IValue
    {
        T Value { get; set; }
    }
}
