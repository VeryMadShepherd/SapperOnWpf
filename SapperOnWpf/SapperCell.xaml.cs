using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SapperOnWpf
{
    /// <summary>
    /// Interaction logic for SapperCell.xaml
    /// </summary>

    public enum CellStatus
    {
        Clear,
        Marked,
        Opened
    }

    public partial class SapperCell : UserControl
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsMined { get; set; }
        public CellStatus Status { get; set; }

        public SapperCell(int row, int column) : base()
        {
            InitializeComponent();
            Row = row;
            Column = column;
            Status = CellStatus.Clear;
        }
        public void SetText(int mineNumber)
        {
            ContentButton.Content = mineNumber.ToString();
            System.Drawing.Color BrushColor;
            switch (mineNumber)
            {
                case 1:
                    BrushColor = Properties.Settings.Default.Color1;
                    break;
                case 2:
                    BrushColor = Properties.Settings.Default.Color2;
                    break;
                case 3:
                    BrushColor = Properties.Settings.Default.Color3;
                    break;
                case 4:
                    BrushColor = Properties.Settings.Default.Color4;
                    break;
                case 5:
                    BrushColor = Properties.Settings.Default.Color5;
                    break;
                case 6:
                    BrushColor = Properties.Settings.Default.Color6;
                    break;
                case 7:
                    BrushColor = Properties.Settings.Default.Color7;
                    break;
                case 8:
                    BrushColor = Properties.Settings.Default.Color8;
                    break;
                default:
                    BrushColor = Properties.Settings.Default.ColorEmpty;
                    break;
            }
            ContentButton.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BrushColor.A, BrushColor.R, BrushColor.G, BrushColor.B));
        }
        public void Mark()
        {
            if (Status == CellStatus.Clear)
            {
                ContentButton.Content = Properties.Resources.Marker;
                Status = CellStatus.Marked;
            }
            else if (Status == CellStatus.Marked)
            {
                ContentButton.Content = null;
                Status = CellStatus.Clear;
            }
        }
        public void Blow(bool wasClicked)
        {
            if (IsMined)
            {
                ContentButton.Content = Properties.Resources.Mine;
                if (wasClicked)
                {
                    System.Drawing.Color BrushColor = System.Drawing.Color.Red;
                    ContentButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BrushColor.A, BrushColor.R, BrushColor.G, BrushColor.B)); 
                }
                return;
            }
        }

        public void Reset()
        {
            ContentButton.Content = "";
            IsMined = false;
            ContentButton.Background = System.Windows.Media.Brushes.LightGray;
            Status = CellStatus.Clear;
        }
    }
}
