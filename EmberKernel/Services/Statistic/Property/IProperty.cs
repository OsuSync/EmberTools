namespace EmberKernel.Services.Statistic.Property
{
    public interface IProperty
    {
        string Name { get; set; }
        PropertyDataType DataType { get; }
        object Data { get; set; }
    }
    public interface IProperty<T> : IProperty
    {
        new T Data { get; set; }
    }
}
