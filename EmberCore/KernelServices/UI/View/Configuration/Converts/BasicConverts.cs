using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace EmberCore.KernelServices.UI.View.Configuration.Converts
{
    public class IntValueConverter : CastToConverter<int>
    {
        public override int Cast(object value) => int.Parse(value.ToString());
    }
    public class DoubleValueConverter : CastToConverter<double>
    {
        public override double Cast(object value) => double.Parse(value.ToString());
    }
    public class BooleanValueConverter : CastToConverter<bool>
    {
        public override bool Cast(object value) => bool.Parse(value.ToString());
    }
}
