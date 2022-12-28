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
    public sealed partial class Cell : UserControl, INotifyPropertyChanged
    {
        public bool isSelected = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public Cell()
        {
            this.InitializeComponent();
        }

        public TextBlock Tb 
        {
            get { return tb; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isSelected = !isSelected;
            Notify();
        }

        protected async void Notify([CallerMemberName] string propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
                );
        }
    }
}
