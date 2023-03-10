using SapperOnWpf.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SapperOnWpf
{
    /// <summary>
    /// Interaction logic for SapperForm.xaml
    /// </summary>
    public partial class SapperForm : Window
    {
        private List<SapperButton> Cells = new List<SapperButton>();
        private List<SapperButton> ClearCells = new List<SapperButton>();
        private Settings Data = Settings.Default;
        private bool IsLocked;
        private int Mines { get; set; }
        private int MarkedMines { get; set; }
        public SapperForm(int rows, int columns, int mines)
        {
            InitializeComponent();
            Mines = mines;

            int width = (columns + 1) * (Data.ButtonSize + Data.ButtonIndent) +
                (2 * Data.XMargin) -
                Data.ButtonIndent;
            int height = (rows + 1) * (Data.ButtonSize + Data.ButtonIndent) +
                (2 * Data.YMargin) +
                Data.UpperBarSize +
                Data.ButtonIndent;
            Width = width;
            Height = height;
            for (int row = 0; row <= rows; row++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int column = 0; column <= columns; column++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int row = 1; row <= rows; row++)
            {
                for (int column = 1; column <= columns; column++)
                {
                    CreateButton(row, column);
                }
            }           

            //ResetButton.Margin = new Thickness(width / 2 - ResetButton.ActualWidth / 2, ResetButton.Margin.Top, ResetButton.Margin.Right, ResetButton.Margin.Bottom);
            //MinesLabel.Margin = new Thickness(width - Data.XMargin - MinesLabel.ActualWidth, MinesLabel.Margin.Top, MinesLabel.Margin.Right, MinesLabel.Margin.Bottom);

            MineCells();
            SetMinesLabel();
        }
        private void SetMinesLabel()
        {
            MarkedMines = Mines;
            MinesLabel.Content = MarkedMines.ToString();
        }
        private void SetMinesLabel(int count)
        {
            MarkedMines = MarkedMines + count;
            MinesLabel.Content = MarkedMines.ToString();
        }
        private void ResetField()
        {
            ClearCells.Clear();
            foreach (var cell in Cells)
            {
                cell.Reset();
                ClearCells.Add(cell);
            }
            MineCells();
            IsLocked = false;
        }
        private void CreateButton(int row, int column)
        {
            var button = new SapperButton(row, column);
            Cells.Add(button);
            ClearCells.Add(button);
            //button.ContentButton.Margin = new Thickness(
            //    Data.XMargin + (Data.ButtonSize + Data.ButtonIndent) * (row - 1),
            //    Data.UpperBarSize + Data.YMargin + (Data.ButtonSize + Data.ButtonIndent) * (column - 1),
            //    0, 0);
            button.Name = "B" + row.ToString() + column.ToString();
            button.AddHandler(MouseLeftButtonUpEvent,
                        new RoutedEventHandler(Cell_MouseLeftUp),
                        true);
            button.AddHandler(MouseRightButtonUpEvent,
                        new RoutedEventHandler(Cell_MouseRightUp),
                        true);
            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);
            MainGrid.Children.Add(button);
        }

        private void Cell_MouseRightUp(object sender, RoutedEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }
            var cell = (SapperButton)sender;
            cell.Mark();
            if (cell.Status == CellStatus.Marked)
            {
                SetMinesLabel(-1);
            }
            else
            {
                SetMinesLabel(1);
            }
        }

        private void Cell_MouseLeftUp(object sender, RoutedEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }
            var cell = (SapperButton)sender;
            if (cell.Status == CellStatus.Marked)
            {
                return;
            }
            if (cell.IsMined)
            {
                Boom(cell);
            }
            else
            {
                OpenCell(cell);
            }
        }

        private void MineCells()
        {
            Random rnd = new Random();
            for (int i = 0; i < Mines; i++)
            {
                int number = rnd.Next(0, ClearCells.Count - 1);
                var cell = ClearCells[number];
                cell.IsMined = true;
                ClearCells.Remove(cell);
            }
        }
        private void Boom(SapperButton clickedCell)
        {
            IsLocked = true;
            foreach (var cell in Cells)
            {
                cell.Blow(cell == clickedCell);
            }
            MessageBox.Show("BOOOOOOOOM!");
        }
        private void OpenCell(SapperButton cell)
        {
            if (cell.Status == CellStatus.Opened)
            {
                return;
            }
            if (cell.Status == CellStatus.Marked)
            {
                cell.Content = "";
            }
            ClearCells.Remove(cell);
            if (ClearCells.Count == 0)
            {
                IsLocked = true;
                MessageBox.Show("You win!");
                return;
            }
            cell.Status = CellStatus.Opened;
            var nearestCells = Cells.FindAll(b =>
            b.Row >= (cell.Row - 1) &&
            b.Row <= (cell.Row + 1) &&
            b.Column >= (cell.Column - 1) &&
            b.Column <= (cell.Column + 1) &&
            b != cell);
            int mines = 0;
            foreach (var nearestCell in nearestCells)
            {
                mines = nearestCell.IsMined ? mines + 1 : mines;
            }
            cell.SetText(mines);
            if (mines == 0)
            {
                foreach (var nearestCell in nearestCells)
                {
                    OpenCell(nearestCell);
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetField();
        }
    }
}
