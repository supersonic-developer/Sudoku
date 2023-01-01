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

        /* Method start a sequence where tries eliminating the given number of cells from filled solution
         */
        public void RandomlyVisitEveryCell()
        {       
            List<ObservableCollection<int>> solutions= new List<ObservableCollection<int>>();
            List<int> requiredForUniquness= new List<int>();    
            List<int> indeces = new List<int>();
            for (int idx = 0; idx < 81; idx++)
            {   indeces.Add(idx);   }
            ObservableCollection<int> cpGame = new ObservableCollection<int>(game);
            int rndIdx;
            while (indeces.Count!=0)
            {
                solsLists.Clear();
                rndIdx=rand.Next(indeces.Count);
                deletedIndeces.Add(indeces[rndIdx]);
                indeces.RemoveAt(rndIdx);
                foreach(int num in deletedIndeces.ToList())
                    game[num] = 0;
                solutions = isUnique(new List<int>(deletedIndeces));
                if (solutions.Count!=1)
                {
                    if (indeces.Count == 0)
                    {
                        
                    }
                    if (solutions.Count == 2)
                    {
                        for (int idx = 0; idx < 81; idx++)
                        {
                            if (solutions[solutions.Count - 1][idx] != solutions[solutions.Count - 2][idx])
                            {
                                requiredForUniquness.Add(idx);
                            }
                        }
                    }
                    if (requiredForUniquness.Count == 1)
                    {
                        game[requiredForUniquness[0]] = cpGame[requiredForUniquness[0]];
                        deletedIndeces.RemoveAt(deletedIndeces.FindIndex(num => num == requiredForUniquness[0]));
                        indeces.Add(deletedIndeces[deletedIndeces.Count - 1]);
                    }
                    requiredForUniquness.Clear();
                    solutions.Clear();
                    game[deletedIndeces[deletedIndeces.Count - 1]] = cpGame[deletedIndeces[deletedIndeces.Count - 1]];
                    deletedIndeces.RemoveAt(deletedIndeces.Count - 1);
                }

                solutions.Clear();
            }
            game = cpGame;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /* Start a sequence which tries to solve a Sudoku
         * Param1: the empty cells which we have to fill up to find solution(s)
         * Ret: if there was only one solution the Sudoku puzzle is proper
         */
        private List<ObservableCollection<int>> isUnique(List<int> emptyCells)
        {
            List<ObservableCollection<int>> solutions= new List<ObservableCollection<int>>();
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
                { 
                    numOfSolutions++;
                    solutions.Add(new ObservableCollection<int>(game));
                }
                // If we find a solution and there is another possible solutions
                if (isOtherPossiblity)
                { 
                    isOtherPossiblity = StepBack(ref emptyCells, ref filledCells);
                    if (isOtherPossiblity)
                    { game[filledCells[filledCells.Count - 1]] = GenerateOKNumber(); }
                }
            }
            return solutions;
        }

        /* Stepping back in the process of filling up the Sudoku with numbers
         * Param1: the already eliminatied cells
         * Param2: the filled cells, which are still in the Sudoku 
         * Ret: if we cant step back means that we tried every possibility
         */
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

        /* Decide if there is a solution for the current Sudoku or not
         * Param1: list of the empty cells' solutions 
         * Comment: it's unsolveable if a position exists where the number of possible solutions are zero
         */
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

        /* Returns a list which contains every possible solutions for the given (empty) cells
         * Param1: the empty cells, which we want to calculate
         */
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

        /*  Returns every possible solution for the given cell
         *  param1: the ID of the cell 
         */
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

        /*  Randomly generate a valid number
         */
        private int GenerateOKNumber()
        {
            int val = sols[rand.Next(sols.Count)];
            sols.RemoveAt(sols.FindIndex(num => num == val));
            return val;
        }

        // ---------------------------------     Basic functions to check Sudoku validity        ---------------------------------- //
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
