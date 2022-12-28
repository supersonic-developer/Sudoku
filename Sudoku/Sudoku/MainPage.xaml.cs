using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sudoku
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public SudokuViewModel sudoku = new SudokuViewModel();

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(1000, 650);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Application.Current.Resources["ToggleSwitchFillOff"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["ToggleSwitchFillOn"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ToggleSwitchFillOnPointerOver"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["ToggleSwitchFillOffPointerOver"] = new SolidColorBrush(Colors.Black);

            Application.Current.Resources["ToggleSwitchStrokeOff"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ToggleSwitchStrokeOn"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["ToggleSwitchStrokeOnPointerOver"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ToggleSwitchStrokeOffPointerOver"] = new SolidColorBrush(Colors.White);

            Application.Current.Resources["ToggleSwitchKnobFillOff"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ToggleSwitchKnobFillOn"] = new SolidColorBrush(Colors.LightGreen);
            Application.Current.Resources["ToggleSwitchKnobFillOffPointerOver"] = new SolidColorBrush(Colors.LightGreen);
            Application.Current.Resources["ToggleSwitchKnobFillOnPointerOver"] = new SolidColorBrush(Colors.Black); 
            sudoku.DisplayedSudoku.CollectionChanged += Game_CollectionChanged;
        }

        private void Game_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sudoku.DisplayEnabled)
            {
                foreach (var strnum in gameBoard.Cells.Zip(sudoku.DisplayedSudoku.Game, (cell, num) => new { Cell = cell, Number = num }))
                {
                    if (strnum.Number > 0)
                        strnum.Cell.Tb.Text = strnum.Number.ToString();
                    else
                        strnum.Cell.Tb.Text = " ";
                }
                sudoku.DisplayedSudoku.Game.CollectionChanged -= Game_CollectionChanged;
                gameBoard.Cells.CollectionChanged += Cells_CollectionChanged;
            }
        }

        private void Cells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            sudoku.DisplayedSudoku.Game.Clear();
            foreach (Cell cell in gameBoard.Cells)
            {   sudoku.DisplayedSudoku.Game.Add(Convert.ToInt32(cell.Tb.Text)); }
            sudoku.DisplayedSudoku.Game.CollectionChanged += Game_CollectionChanged;
            gameBoard.Cells.CollectionChanged -= Cells_CollectionChanged;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void viewMode_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void supervisorMode_Toggled(object sender, RoutedEventArgs e)
        {

        }
    }
}
