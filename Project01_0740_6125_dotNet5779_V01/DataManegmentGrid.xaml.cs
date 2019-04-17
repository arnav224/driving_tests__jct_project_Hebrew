using System;
using System.Collections.Generic;
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

namespace Project01_0740_6125_dotNet5779_V01
{
    /// <summary>
    /// Interaction logic for DataManegmentGrid.xaml
    /// </summary>
    public partial class DataManegmentGrid : UserControl
    {
        public DataManegmentGrid()
        {
            InitializeComponent();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName + "/Data/logo.png");
            bitmapImage.EndInit();
            this.logoImage.Source = bitmapImage;

        }
    }
}
