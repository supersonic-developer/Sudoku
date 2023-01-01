using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{

    public class SudokuViewModel : INotifyPropertyChanged
    {
        Sudoku sudoku = new Sudoku();
        int sudokuSize = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public SudokuViewModel()
        {
            sudoku.Game.CollectionChanged += Game_CollectionChanged;
            Task.Run(() => sudoku.CreateGame());
        }

        public Sudoku Sudoku
        { 
            get { return sudoku; }
        }

        protected async void Notify([CallerMemberName] string propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
                );
        }

        private void Game_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SudokuSize = sudoku.Game.Count;
            if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Add && sudoku.Game.Count == 81)
                Task.Run(() => sudoku.RandomlyVisitEveryCell());
        }

        public int SudokuSize
        {
            get {   return sudokuSize;  }
            set 
            { 
                sudokuSize = value;
                Notify();
            }
        }
    }
}
