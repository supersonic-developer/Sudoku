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
    public class SelectedCellConverter : IValueConverter
    {
        readonly private static SolidColorBrush lightGray = new SolidColorBrush(Colors.LightGray);
        readonly private static SolidColorBrush white = new SolidColorBrush(Colors.White);
        readonly private static SolidColorBrush lightGreen = new SolidColorBrush(Colors.LightGreen);
        readonly private static SolidColorBrush black = new SolidColorBrush(Colors.Black);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value && !MainPage.isDarkMode)
                return lightGray;
            else if (!(bool)value && !MainPage.isDarkMode)
                return white;
            else if ((bool)value && MainPage.isDarkMode)
                return lightGreen;
            else
                return black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
