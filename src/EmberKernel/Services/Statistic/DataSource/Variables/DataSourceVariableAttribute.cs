using System;

namespace EmberKernel.Services.Statistic.DataSource.Variables
{
    public class DataSourceVariableAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }
        public string Id { get; set; } = null;
        public string Namespace { get; set; } = null;
        public DataSourceVariableAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
