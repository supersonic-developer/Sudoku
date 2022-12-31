using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class ThemeViewModel : INotifyPropertyChanged
    {
        int borderBrush;
        int background ;
        int foreground;
        public event PropertyChangedEventHandler PropertyChanged;

        public ThemeViewModel(int border, int bgnd, int fgnd)
        {
            borderBrush= border;
            background= bgnd;   
            foreground= fgnd;
        }

        private async void Notify([CallerMemberName] string propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
                );
        }

        public int BorderBrush
        {
            get { return borderBrush; }
            set 
            {
                borderBrush = value;
                Notify();
            }
        }

        public int Background
        {
            get { return background; }
            set
            {
                background = value;
                Notify();
            }
        }

        public int Foreground
        {
            get { return foreground; }
            set
            {
                foreground = value;
                Notify();
            }
        }
    }
}
