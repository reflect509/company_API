using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp
{
    public class SelectedTabColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var current = value?.ToString()?.ToLowerInvariant();
            var target = parameter?.ToString()?.ToLowerInvariant();

            // Если routes совпадают — желтый, иначе — красный
            return current == target
                ? Colors.Yellow
                : Colors.Red;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
