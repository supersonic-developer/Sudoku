using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.UI;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class GameBoard : UserControl
    {
        List<Cell> cells = new List<Cell>();
        Cell prevSelected;
        SudokuViewModel sudokuVM = new SudokuViewModel();
        ObservableCollection<Border> bigCells=new ObservableCollection<Border>();

        public GameBoard()
        {
            this.InitializeComponent();  
            for (int id = 0; id < 81; id++)
            { 
                cells.Add(new Cell());
                Grid.SetColumn(cells[id], id/9);
                Grid.SetRow(cells[id], id%9);
                board.Children.Add(cells[id]);
            }
            SolidColorBrush borderColor=new SolidColorBrush(Colors.Black);
            // Draw big cells
            for (int col = 0; col < 9; col += 3)
            {
                for (int row = 0; row < 9; row += 3)
                {
                    bigCells.Add(new Border());
                    Binding binding = new Binding();
                    binding.Source = cells[0].ThemeViewModel.BorderBrush;
                    binding.Converter = new Code2ColorConverter();
                    bigCells[bigCells.Count - 1].SetBinding(Border.BorderBrushProperty, binding);
                    bigCells[bigCells.Count - 1].BorderThickness = new Thickness(3);
                    Grid.SetColumn(bigCells[bigCells.Count - 1], col);
                    Grid.SetRow(bigCells[bigCells.Count - 1], row);
                    Grid.SetColumnSpan(bigCells[bigCells.Count - 1], 3);
                    Grid.SetRowSpan(bigCells[bigCells.Count - 1], 3);
                    board.Children.Add(bigCells[bigCells.Count - 1]);
                }
            }
            sudokuVM.Sudoku.CollectionChanged += Game_CollectionChanged;
            foreach (Cell cell in cells)
            { 
                cell.CellViewModel.PropertyChanged += Cell_PropertyChanged;
                cell.ThemeViewModel.PropertyChanged += Theme_PropertyChanged;
            }
        }

        public SudokuViewModel SudokuVM
        {
            get { return sudokuVM; }
        }

        public List<Cell> Cells 
        {
            get { return cells; }
        }

        private void Game_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var strnum in cells.Zip(sudokuVM.Sudoku.Game, (cell, num) => new { Cell = cell, Number = num }))
            {   strnum.Cell.CellViewModel.Content = strnum.Number.ToString();     }
            foreach (int emptyCell in sudokuVM.Sudoku.DeletedIndeces)
            {   cells[emptyCell].CellViewModel.Content = "";   }
        }

        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CellViewModel cellVM=(CellViewModel)sender;
            if (e.PropertyName == "Content")
            {
                if (cellVM.Content != "")
                    sudokuVM.Sudoku.Game[cells.FindIndex(cell => cell.CellViewModel == cellVM)] = Convert.ToInt32(cellVM.Content);
                else
                    sudokuVM.Sudoku.Game[cells.FindIndex(cell => cell.CellViewModel == cellVM)] = 0;
            }
            else if (e.PropertyName == "IsSelected")
            {
                foreach (Cell cell in cells)
                {
                    if (cell.CellViewModel != cellVM)
                    {
                        if (cell.ThemeViewModel.Background == 0)
                            cell.Btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.LightGray);
                        else if (cell.ThemeViewModel.Background == 1)
                            cell.Btn.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Colors.LightGreen);
                    }
                    else 
                    {

                    }
                }
            }
        }

        private void Theme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
