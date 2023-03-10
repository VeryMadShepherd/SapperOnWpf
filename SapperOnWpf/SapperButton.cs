using SapperOnWpf.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SapperOnWpf
{
    internal class SapperButton : Button
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsMined { get; set; }
        public CellStatus Status { get; set; }
        private Settings Data = Settings.Default;

        public SapperButton(int row, int column) : base()
        {
            Row = row;
            Column = column;
            Status = CellStatus.Clear;
            Height = Data.ButtonSize;
            Width = Data.ButtonSize;
            Content = "";
            FontFamily = new FontFamily("Microsoft Sans Serif");
            FontSize = 20;
            Background = Brushes.LightGray;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
        public void SetText(int mineNumber)
        {
            Content = mineNumber.ToString();
            System.Drawing.Color BrushColor;
            Background = Brushes.DarkGray;
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
                    Content = "";
                    BrushColor = Properties.Settings.Default.ColorEmpty;
                    Background = Brushes.Gray;
                    break;
            }
            Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BrushColor.A, BrushColor.R, BrushColor.G, BrushColor.B));
        }
        public void Mark()
        {
            if (Status == CellStatus.Clear)
            {
                SetImageContent("Marker");
                Status = CellStatus.Marked;
            }
            else if (Status == CellStatus.Marked)
            {
                Content = null;
                Status = CellStatus.Clear;
            }
        }
        public void Blow(bool wasClicked)
        {
            if (IsMined)
            {
                SetImageContent("Mine");
                if (wasClicked)
                {
                    Background = Brushes.Red;
                }
                return;
            }
        }

        public void Reset()
        {
            Content = "";
            IsMined = false;
            Background = System.Windows.Media.Brushes.LightGray;
            Status = CellStatus.Clear;
        }

        private void SetImageContent(string imageName)
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"/Resources/" + imageName + ".png", UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            Image img = new Image();
            img.Stretch = Stretch.Fill;
            img.Source = bitimg;
            Content = img;
        }
    }
}
