namespace EmberKernel.Services.Statistic.Property
{
    public struct StringProperty : IProperty<string>
    {
        public PropertyDataType DataType => PropertyDataType.String;
        public string Data { get; set; }
        public string Name { get; set; }
        object IProperty.Data { get => Name; set => Name = value.ToString(); }

        public override string ToString()
        {
            return Data;
        }
    }
}
