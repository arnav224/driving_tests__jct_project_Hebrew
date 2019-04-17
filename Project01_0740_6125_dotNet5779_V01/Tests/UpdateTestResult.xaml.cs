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
    public partial class UpdateTestResult 
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test = ((MainWindow)System.Windows.Application.Current.MainWindow).selectedTests[0];
        List<string> errorMessages = new List<string>();

        /// <summary>
        /// UpdateTestResult ctor
        /// </summary>
        public UpdateTestResult()
        {
            InitializeComponent();
            this.DataContext = test;
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
            switch (test.Indices["שליטה בהגה"])
            {
                case BE.Score.נכשל:
                    GrudRow2.RowScore0.IsChecked = true;
                    break;
                case BE.Score.רע_מאוד:
                    GrudRow2.RowScore1.IsChecked = true;
                    break;
                case BE.Score.רע:
                    GrudRow2.RowScore2.IsChecked = true;
                    break;
                case BE.Score.עבר:
                    GrudRow2.RowScore3.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Button Click to save the Update Test Result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            test.TesterNotes = this.TesterNotesTextBox.Text;
            bl.UpdateTestResult(test);
            this.Closing -= Window_Closing;
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.test != bl.GetAllTests(t => t.TestID == test.TestID).FirstOrDefault())//@@יש למצוא דרך להשוות בלי tostring
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }
    }
}
