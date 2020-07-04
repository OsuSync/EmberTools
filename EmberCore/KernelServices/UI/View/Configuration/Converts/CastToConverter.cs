using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace EmberCore.KernelServices.UI.View.Configuration.Converts
{
    public abstract class CastToConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Cast(value);

        public abstract T Cast(object value);
    }
}
