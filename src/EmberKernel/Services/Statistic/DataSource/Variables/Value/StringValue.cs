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

        public static implicit operator StringValue(string str)
        {
            return new StringValue(str);
        }
    }
}
