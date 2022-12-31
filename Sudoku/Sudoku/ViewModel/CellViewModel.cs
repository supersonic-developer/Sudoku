using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class CellViewModel : INotifyPropertyChanged
    {
        bool isSelected = false;
        string content;
        public event PropertyChangedEventHandler PropertyChanged;

        protected async void Notify([CallerMemberName] string propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
                );
        }

        public string Content
        {
            get { return content; }
            set 
            {
                if (value != content)
                {
                    content = value;
                    Notify();
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set 
            {
                isSelected = value;
                Notify();
            }
        }
    }
}
