using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Vlaplom.Converters
{
    /// <summary>
    /// Конвертер, преобразующий получаемый enum в строчку.
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "None";

            // Нужно использовать выражение "[$"{value}"]", так как Resources[] принимает string, а value -- enum.
            return Application.Current.Resources[$"{value}"]?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
