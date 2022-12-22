using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Sudoku
    {
        ObservableCollection<int> game = new ObservableCollection<int>();
        Random rand = new Random();
        List<int> triedNumbers = new List<int>();
        List<int> sols = new List<int>();
        List<List<int>> solsLists = new List<List<int>>();
        List<List<int>> triedLists = new List<List<int>>();

        public ObservableCollection<int> Game
        {
            get 
            {
                return game;
            }
        }

        public void CreateGame()
        {
            while (game.Count < 81)
            {
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

        private void CalculateGoodNums(int id)
        {
            solsLists.Add(new List<int>());
            sols = solsLists[solsLists.Count - 1];
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
            int rndIdx, val;
            rndIdx = rand.Next(sols.Count);
            val = sols[rndIdx];
            sols.RemoveAt(rndIdx);
            return val;
        }

        private void CheckRow(int id)
        {
            try
            {
                int firstElement = 9 * (id / 9);
                int idx;
                for (int cnt = firstElement; cnt < firstElement+9; cnt++)
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
                for (int cnt = firstElement; cnt < firstElement+81; cnt+=9)
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
                int startId=CalculateId(row, col);
                int idx;
                for (int rowIdx = 0; rowIdx < 27; rowIdx+=9)
                {
                    for (int colIdx = 0; colIdx < 3; colIdx++)
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

        private int CalculateId(int row, int col)
        {
            return (9 * row + col);
        }
    }
}
