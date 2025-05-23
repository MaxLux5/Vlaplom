using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Vlaplom.Converters
{
    /// <summary>
    /// Конвертер, преобразующий получаемый TimeSpan в Duration. Если TimeSpan is null, то Duration будет нулевым.
    /// </summary>
    class TimeSpanToDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
                return new Duration(timeSpan);

            return new Duration();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
