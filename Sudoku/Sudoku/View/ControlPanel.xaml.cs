using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ControlPanel : UserControl
    {
        ThemeViewModel themeViewModel = new ThemeViewModel(1,0,1);
        public ControlPanel()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonText = (string)((Button)sender).Name;
            switch (buttonText)
            {
                case "btn1":
                    break;
                case "btn2":
                    break;
                case "btn3":
                    break;
                case "btn4":
                    break;
                case "btn5":
                    break;
                case "btn6":
                    break;
                case "btn7":
                    break;
                case "btn8":
                    break;
                case "btn9":
                    break;
                case "btnUndo":
                    break;
                case "btnRevert":
                    break;
                default:
                    break;
            }
        }
    }
}
