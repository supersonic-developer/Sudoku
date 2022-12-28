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
        readonly private static SolidColorBrush lightGray = new SolidColorBrush(Colors.Gray);
        readonly private static SolidColorBrush white = new SolidColorBrush(Colors.White);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return lightGray;
            }
            else
            {
                return white;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
