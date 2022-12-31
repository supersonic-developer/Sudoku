using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Sudoku
{
    public sealed partial class Cell : UserControl
    {
        public CellViewModel CellViewModel=new CellViewModel();
        public ThemeViewModel ThemeViewModel=new ThemeViewModel(1,0,1);

        public Cell()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CellViewModel.IsSelected = !CellViewModel.IsSelected;
            if (ThemeViewModel.Background < 2)
                ThemeViewModel.Background += 2;
            else if (ThemeViewModel.Background > 1)
                ThemeViewModel.Background -= 2;
            if (ThemeViewModel.Background == 2)
                btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.White);
            else if (ThemeViewModel.Background == 3)
                btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.Black);
        }

        public Button Btn
        {
            get { return btn; }
        }
    }
}
