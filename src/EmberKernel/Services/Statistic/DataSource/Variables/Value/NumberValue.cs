namespace EmberKernel.Services.Statistic.DataSource.Variables.Value
{
    public struct NumberValue : IValue<double>
    {
        public double Value { get; set; }
        public ValueType Type => ValueType.Number;
        public NumberValue(double value)
        {
            Value = value;
        }

        public static NumberValue Default => new NumberValue(0);

        public static implicit operator NumberValue(double value)
        {
            return new NumberValue(value);
        }

        public bool Equals(IValue<double> x, IValue<double> y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(IValue<double> obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}
