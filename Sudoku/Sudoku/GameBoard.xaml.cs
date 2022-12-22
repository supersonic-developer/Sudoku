﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public Sudoku solution = new Sudoku();
        List<Cell> cells = new List<Cell>();

        public GameBoard()
        {
            this.InitializeComponent();
            Task.Run(() => solution.CreateGame());
            for (int id = 0; id < 81; id++)
            { 
                cells.Add(new Cell());
                Grid.SetColumn(cells[id], id/9);
                Grid.SetRow(cells[id], id%9);
                board.Children.Add(cells[id]);
                //cells[id].Tb.Text = solution.Game[id].ToString();
            }
        }
    }
}
