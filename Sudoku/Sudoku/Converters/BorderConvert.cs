using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Sudoku
{
    public class BorderConvert : IValueConverter
    {
        readonly private static SolidColorBrush white = new SolidColorBrush(Colors.White);
        readonly private static SolidColorBrush black = new SolidColorBrush(Colors.Black);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return white;
            else
                return black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
