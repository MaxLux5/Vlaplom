using System.Globalization;
using System.Windows.Data;

namespace Vlaplom.Converters
{
    /// <summary>
    /// Конвертер, преобразующий получаемое значение bool на противоположное.
    /// </summary>
    class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // В выражении "value is bool b", если приведение к типу проходит успешно, то значение из value сохраняется в переменную b.
            // 
            // Оператор && производит операцию логического умножения.
            // Возвращает true, если оба операнда одновременно равны true, иначе возвращает false
            return !(value is bool b && b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool b && b);
        }
    }
}
