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
    /// Interaction logic for AppeaTests.xaml
    /// </summary>
    public partial class AppeaTests : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();

        public AppeaTests()
        {
            InitializeComponent();
            ApplyFiltering(this, new RoutedEventArgs());
        }

        private void ApplyFiltering(object sender, RoutedEventArgs e)
        {
            this.AppealsDataGrid.ItemsSource = from item in bl.GetAllTests(t => t.AppealTest != null)
                                               select new { TestID = item.TestID, TraineeID = item.TraineeID, TesterID = item.TesterID,
                                                   RequestTime = item.AppealTest.RequestTime, DecisionTime = item.AppealTest.DecisionTime,
                                                   AppealStatus = item.AppealTest.appealStatus, TraineeNotes = item.AppealTest.TraineeNotes,
                                                   Decision = item.AppealTest.Decision };
        }

        private void AppealsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TestID":
                    e.Column.Header = "";
                    break;
                case "TraineeID":
                    e.Column.Header = "";
                    break;
                case "TesterID":
                    e.Column.Header = "";
                    break;
                case "RequestTime":
                    e.Column.Header = "";
                    break;
                case "DecisionTime":
                    e.Column.Header = "";
                    break;
                case "AppealStatus":
                    e.Column.Header = "";
                    break;
                case "TraineeNotes":
                    e.Column.Header = "";
                    break;
                case "Decision":
                    e.Column.Header = "";
                    break;

                default:
                    break;
            }
        }
    }
}
