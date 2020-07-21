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

        public static implicit operator NumberValue(double value)
        {
            return new NumberValue(value);
        }
    }
}
