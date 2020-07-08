namespace EmberKernel.Services.Statistic.Property
{
    public struct NumberProperty : IProperty<double>
    {
        public PropertyDataType DataType => PropertyDataType.Number;
        public double Data { get; set; }
        public string Name { get; set; }
        object IProperty.Data { get => Data; set => double.Parse(value.ToString()); }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
