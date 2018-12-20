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
using System.Windows.Shapes;

namespace Project01_0740_6125_dotNet5779_V01
{
    /// <summary>
    /// Interaction logic for UpdateTestResult.xaml
    /// </summary>
    public partial class UpdateTestResult : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test;
        List<string> errorMessages = new List<string>();
        public UpdateTestResult()
        {
            InitializeComponent();
            test = bl.GetAllTests(t => t.TestID == Convert.ToInt32(((MainWindow)System.Windows.Application.Current.MainWindow).TestsTabUserControl.Update_IdTextBox.Text)).FirstOrDefault();
            this.DataContext = test;
            if (test.Indices == null)
                test.Indices = BE.Configuration.DefultIndices();
            int index = 0;
            var indicesInumerator = test.Indices.GetEnumerator();
            foreach (dynamic item in (from UIElement item in this.MeinGrid.Children where item is GridRow select item as GridRow))
            {
                GridRow gridRow = item as GridRow;
                indicesInumerator.MoveNext();
                switch (indicesInumerator.Current.Value)
                {
                    case BE.Score.נכשל:
                        gridRow.RowScore0.IsChecked = true;
                        break;
                    case BE.Score.רע_מאוד:
                        gridRow.RowScore1.IsChecked = true;
                        break;
                    case BE.Score.רע:
                        gridRow.RowScore2.IsChecked = true;
                        break;
                    case BE.Score.עבר:
                        gridRow.RowScore3.IsChecked = true;
                        break;
                    default:
                        break;
                }
                gridRow.RowScore0.GroupName = gridRow.RowScore1.GroupName = gridRow.RowScore2.GroupName = gridRow.RowScore3.GroupName = index++.ToString();
                gridRow.LableRow.Content = indicesInumerator.Current.Key;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (dynamic item in (from UIElement item in this.MeinGrid.Children where item is GridRow select item as GridRow))
            {
                GridRow gridRow = item as GridRow;
                if ((bool)gridRow.RowScore0.IsChecked)
                    test.Indices[(string)gridRow.LableRow.Content] = (BE.Score)0;
                else if((bool)gridRow.RowScore1.IsChecked)
                    test.Indices[(string)gridRow.LableRow.Content] = (BE.Score)1;
                else if ((bool)gridRow.RowScore2.IsChecked)
                    test.Indices[(string)gridRow.LableRow.Content] = (BE.Score)2;
                else if((bool)gridRow.RowScore3.IsChecked)
                    test.Indices[(string)gridRow.LableRow.Content] = (BE.Score)3;
            }
            test.TesterNotes = this.TesterNotesTextBlock.Text;
            bl.UpdateTestResult(test);
            this.Close();
        }
    }
}
