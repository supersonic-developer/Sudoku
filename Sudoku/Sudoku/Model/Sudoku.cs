using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing.OptionDetails;

namespace Sudoku
{
    public class Sudoku : INotifyCollectionChanged
    {
        ObservableCollection<int> game = new ObservableCollection<int>();
        List<int> deletedIndeces = new List<int>();
        Random rand = new Random();
        List<int> sols = new List<int>();
        List<List<int>> solsLists = new List<List<int>>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollection<int> Game
        {
            get {   return game;    }
            set {   game= value;    }
        }

        public List<int> DeletedIndeces
        {
            get { return deletedIndeces; }
        }

        protected async void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal,
                // EVIP: ?. for invoking event. Event is null if there are not subscribers.
                () => { CollectionChanged?.Invoke(this, e); }
                );
        }

        public void CreateGame()
        {
            while (game.Count < 81)
            {
                solsLists.Add(new List<int>());
                sols = solsLists[solsLists.Count - 1];
                CalculateGoodNums(game.Count);

                // If we tried every number back one step
                while (sols.Count == 0)
                {
                    solsLists.RemoveAt(solsLists.Count - 1);
                    sols = solsLists[solsLists.Count - 1];
                    game.RemoveAt(game.Count - 1);
                }
                game.Add(GenerateOKNumber());
            }
        }

        public void RandomlyDeleteCells(int numOfDeletingCells)
        {       
            List<int> indeces = new List<int>();
            for (int idx = 0; idx < 81; idx++)
            {
                indeces.Add(idx);
            }
            ObservableCollection<int> cpGame = new ObservableCollection<int>(game);

            while (deletedIndeces.Count < numOfDeletingCells)
            {
                solsLists.Clear();
                deletedIndeces.Add(indeces[rand.Next(indeces.Count)]);
                indeces.RemoveAt(indeces.FindIndex(num => num == deletedIndeces[deletedIndeces.Count - 1]));
                foreach(int num in deletedIndeces.ToList())
                    game[num] = 0;

                if (!isUnique(new List<int>(deletedIndeces)))
                {
                    game[deletedIndeces[deletedIndeces.Count - 1]] = cpGame[deletedIndeces[deletedIndeces.Count - 1]];
                    deletedIndeces.RemoveAt(deletedIndeces.Count - 1);
                    if (deletedIndeces.Count + indeces.Count < numOfDeletingCells)
                    {
                        indeces.Clear();
                        for (int idx = 0; idx < 81; idx++)
                        { indeces.Add(idx); }
                        int removedCellsCount = deletedIndeces.Count;
                        int rndIdx;
                        for (int cnt = 0; cnt < removedCellsCount / 2; cnt++)
                        {
                            rndIdx = rand.Next(deletedIndeces.Count);
                            game[deletedIndeces[rndIdx]] = cpGame[deletedIndeces[rndIdx]];
                            deletedIndeces.RemoveAt(rndIdx);  
                        }
                        foreach (int idx in deletedIndeces)
                        { indeces.RemoveAt(indeces.FindIndex(num => num == idx)); }
                    }
                }
            }
            game = cpGame;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private bool isUnique(List<int> emptyCells)
        {
            List<List<int>> numOfPossibleSols;
            List<int> filledCells = new List<int>();
            int leastSolIdx;
            int numOfEmptyCells = emptyCells.Count;
            int numOfSolutions = 0;
            bool isOtherPossiblity = true;
            // Loop until we have possible solutions or we find at least 2 solution
            while (numOfSolutions<2 && isOtherPossiblity)
            {
                while (filledCells.Count < numOfEmptyCells && isOtherPossiblity)
                {
                    // Find possible solutions for every empty cells
                    numOfPossibleSols = SearchAllPossibleSolutions(emptyCells);
                    // All cell can be filled correctly
                    if (isThereSolution(numOfPossibleSols))
                    {
                        leastSolIdx = numOfPossibleSols.FindIndex(list => list.Count == numOfPossibleSols.Min(element => element.Count));
                        sols = numOfPossibleSols[leastSolIdx];
                        filledCells.Add(emptyCells[leastSolIdx]);
                        game[emptyCells[leastSolIdx]] = GenerateOKNumber();
                        emptyCells.RemoveAt(leastSolIdx);
                        solsLists.Add(new List<int>(sols));
                    }
                    else
                    {
                        // Step back, if we cant then there is no other possiblity
                        isOtherPossiblity=StepBack(ref emptyCells, ref filledCells);
                        if(isOtherPossiblity) 
                        { game[filledCells[filledCells.Count - 1]] = GenerateOKNumber(); }
                    }
                }
                // We find a solution
                if (filledCells.Count == numOfEmptyCells)
                { numOfSolutions++; }
                // If we find a solution and there is another possible solutions
                if (isOtherPossiblity)
                { 
                    isOtherPossiblity = StepBack(ref emptyCells, ref filledCells);
                    if (isOtherPossiblity)
                    { game[filledCells[filledCells.Count - 1]] = GenerateOKNumber(); }
                }
            }
            if (numOfSolutions == 1)
                return true;
            else
                return false;
        }

        private bool StepBack(ref List<int> emptyCells, ref List<int> filledCells)
        {
            sols = solsLists[solsLists.Count - 1];
            while (sols.Count == 0 && solsLists.Count > 1)
            {
                solsLists.RemoveAt(solsLists.Count - 1);
                sols = solsLists[solsLists.Count - 1];
                game[filledCells[filledCells.Count - 1]] = 0;
                emptyCells.Add(filledCells[filledCells.Count - 1]);
                filledCells.RemoveAt(filledCells.Count - 1);
            }
            if (sols.Count == 0)
            { return false; }
            else { return true; }
        }

        private bool isThereSolution(List<List<int>> lPossibleSolutions)
        {
            foreach (List<int> numOfSolutions in lPossibleSolutions)
            {
                if (numOfSolutions.Count < 1)
                {
                    return false;
                }
            }
            return true;
        }

        private List<List<int>> SearchAllPossibleSolutions(List<int> emptyCells)
        {
            List<List<int>> lPossibleSolutions = new List<List<int>>();
            foreach (int cellIdx in emptyCells)
            {
                lPossibleSolutions.Add(new List<int>());
                sols = lPossibleSolutions[lPossibleSolutions.Count - 1];
                CalculateGoodNums(cellIdx);
            }
            return lPossibleSolutions;
        }

        private void CalculateGoodNums(int id)
        {
            for (int num = 1; num < 10; num++)
            {
                sols.Add(num);
            }
            CheckCell(id);
            CheckRow(id);
            CheckColumn(id);
        }

        private int GenerateOKNumber()
        {
            int val = sols[rand.Next(sols.Count)];
            sols.RemoveAt(sols.FindIndex(num => num == val));
            return val;
        }

        private void CheckRow(int id)
        {
            try
            {
                int firstElement = 9 * (id / 9);
                int idx;
                for (int cnt = firstElement; cnt < firstElement+9 && cnt<game.Count; cnt++)
                {
                    idx = sols.FindIndex(num => num == game[cnt]);
                    if (idx != -1)
                    {
                        sols.RemoveAt(idx);
                    }
                }
            }
            catch (Exception){}
        }

        private void CheckColumn(int id)
        {
            try
            {
                int firstElement = id % 9;
                int idx;
                for (int cnt = firstElement; cnt < firstElement+81 && cnt < game.Count; cnt+=9)
                {
                    idx = sols.FindIndex(num => num == game[cnt]);
                    if (idx != -1)
                    {
                        sols.RemoveAt(idx);
                    }
                }
            }
            catch (Exception){}
        }

        private void CheckCell(int id)
        {
            try
            {
                int row = id / 9;
                row -= (row % 3);
                int col = id % 9;
                col -= (col % 3);
                int startId= 9 * row + col;
                int idx;
                for (int rowIdx = 0; rowIdx < 27; rowIdx+=9)
                {
                    for (int colIdx = 0; colIdx < 3 && startId + rowIdx + colIdx<game.Count; colIdx++)
                    {
                        idx = sols.FindIndex(num => num == game[startId+rowIdx+colIdx]);
                        if (idx != -1)
                        { 
                            sols.RemoveAt(idx);
                        }
                    }
                }
            }
            catch (Exception){}
        }
    }
}
