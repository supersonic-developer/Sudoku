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
        Sudoku solution = new Sudoku();
        bool displayEnabled = false;
        int sudokuSize = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        public Sudoku DisplayedSudoku = new Sudoku();

        protected async void Notify([CallerMemberName] string propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
                );
        }

        public SudokuViewModel() 
        {
            solution.Game.CollectionChanged += SolutionGame_CollectionChanged; 
            Task.Run(() => solution.CreateGame());
        }

        private void SolutionGame_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SudokuSize = solution.Game.Count;
            if (solution.Game.Count == 81)
            {
                Task.Run(() => DisplayedSudoku.RandomlyDeleteCells(40, solution.Game, ref displayEnabled)); 
            }   
        }

        public int SudokuSize
        {
            get {   return sudokuSize;  }
            set 
            {
                Notify(); 
                sudokuSize = value; 
            }
        }

        public bool DisplayEnabled
        {
            get { return displayEnabled; }
        }
    }
}
