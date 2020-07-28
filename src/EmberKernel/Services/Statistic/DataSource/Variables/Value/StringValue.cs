namespace EmberKernel.Services.Statistic.DataSource.Variables.Value
{
    public struct StringValue : IValue<string>
    {
        public ValueType Type => ValueType.String;

        public string Value { get; set; }

        public StringValue(string value)
        {
            Value = value;
        }

        public static StringValue Default => new StringValue(string.Empty);

        public static implicit operator StringValue(string str)
        {
            return new StringValue(str);
        }

        public bool Equals(IValue<string> x, IValue<string> y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(IValue<string> obj)
        {
            return obj.Value.GetHashCode();
        }

        public string GetString()
        {
            return Value;
        }
    }
}
