using System;
using System.Globalization;
using System.Windows.Data;

namespace Chess
{
    public class BoolToBlackWhiteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? mybool = value as bool?;

            if (mybool != null && mybool.Value)
            {
                return ConsoleColor.Black;
            }
            return ConsoleColor.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
