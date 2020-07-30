using System;

namespace EmberKernel.Services.Statistic.DataSource.Variables
{
    public class DataSourceVariableAttribute : Attribute
    {
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public string Id { get; set; } = null;
    }
}
