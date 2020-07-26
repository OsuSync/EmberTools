using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl.FormatExpression
{
    public abstract class ValueBase
    {
        public enum Type
        {
            Number,String
        }

        public abstract Type ValueType { get; }

        public static StringValue Create(string str) => new StringValue(str);
        public static NumberValue Create(double value) => new NumberValue(value);
        public static ValueBase Create(ValueBase value) => value;

        public static implicit operator double(ValueBase value)
        {
            return value.ValueType == Type.Number ? ((NumberValue)value).Value : double.NaN;
        }

        public abstract string ValueToString();
    }

    public class NumberValue : ValueBase
    {
        public static NumberValue Zero { get; } = Create(0);
        public static NumberValue One { get; } = Create(1);
        public static NumberValue Nan { get; } = Create(double.NaN);

        public NumberValue(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override Type ValueType => Type.Number;

        public override string ValueToString() => Value.ToString();
    }


    public class StringValue : ValueBase
    {
        public StringValue(string str)
        {
            Value = str;
        }

        public string Value { get; }

        public override Type ValueType => Type.String;

        public override string ValueToString() => Value.ToString();
    }
}
