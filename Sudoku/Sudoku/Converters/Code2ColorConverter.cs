using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Sudoku
{
    public class Code2ColorConverter : IValueConverter
    {
        readonly private static SolidColorBrush lightGray = new SolidColorBrush(Colors.LightGray);
        readonly private static SolidColorBrush white = new SolidColorBrush(Colors.White);
        readonly private static SolidColorBrush lightGreen = new SolidColorBrush(Colors.LightGreen);
        readonly private static SolidColorBrush black = new SolidColorBrush(Colors.Black);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int code = (int)value;
            switch (code)
            {
                case 0:
                    return white;
                case 1:
                    return black;
                case 2:
                    return lightGray;
                case 3:
                    return lightGreen;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
