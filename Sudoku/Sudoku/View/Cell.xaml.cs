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

        public Cell()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CellViewModel.IsSelected = !CellViewModel.IsSelected;
            if(MainPage.isDarkMode)
                btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.Black);
            else
                btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.White);
        }

        public Button Btn
        {
            get { return btn; }
        }
    }
}
