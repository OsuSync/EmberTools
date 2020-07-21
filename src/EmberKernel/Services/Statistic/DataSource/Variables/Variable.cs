using EmberKernel.Services.Statistic.DataSource.Variables.Value;
using System;
using System.Reflection;

namespace EmberKernel.Services.Statistic.DataSource.Variables
{
    public struct Variable
    {
        public string Namespace { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IValue Value { get; set; }

        public static bool IsNumbericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
        public static Variable CreateFrom(Type type)
        {
            if (IsNumbericType(type))
            {
                return new Variable()
                {
                    Value = new NumberValue()
                };
            }
            return new Variable() { Value = new StringValue() };
        }

        public static Variable CreateFrom(PropertyInfo property)
        {
            var rawVariable = CreateFrom(property.PropertyType);
            rawVariable.Id = property.Name;
            rawVariable.Namespace = property.DeclaringType.Namespace;
            if (property.GetCustomAttribute<DataSourceVariableAttribute>() is DataSourceVariableAttribute attr)
            {
                rawVariable.Name = attr.Name;
                rawVariable.Description = attr.Description;
                if (attr.Namespace != null)
                {
                    rawVariable.Namespace = attr.Namespace;
                }
                if (attr.Id != null)
                {
                    rawVariable.Id = attr.Id;
                }
            }
            else
            {
                rawVariable.Name = rawVariable.Id;
                rawVariable.Description = string.Empty;
            }

            return rawVariable;
        }
    }
}
