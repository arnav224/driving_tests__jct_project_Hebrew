﻿using System;
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
        public List<BE.Test> selectedTests = new List<BE.Test>();

        public AppeaTests()
        {
            InitializeComponent();
            ApplyFiltering(this, new RoutedEventArgs());
            this.AppealsDataGrid.SelectionChanged += AppealsDataGrid_SelectionChanged;
            this.DesisionButton.Click += DesisionButton_Click;
            this.SearchTextBox.TextChanged += ApplyFiltering;
            this.FromTimeDatePicker.LostFocus += ApplyFiltering;
            this.ToTimeDatePicker.LostFocus += ApplyFiltering;
            this.StatusComboBox.SelectionChanged += ApplyFiltering;
            this.ResetFiltersButton.Click += ResetFiltersButton_Click;
            this.StatusComboBox.ItemsSource = Enum.GetValues(typeof(BE.AppealStatus));
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            this.FromTimeDatePicker.SelectedDate = null;
            this.ToTimeDatePicker.SelectedDate = null;
            this.SearchTextBox.Text = "";
            this.StatusComboBox.SelectedItem = null;
            ApplyFiltering(this, e);
        }

        private void DesisionButton_Click(object sender, RoutedEventArgs e)
        {
            new AppealDecision().ShowDialog();
            ApplyFiltering(this, new RoutedEventArgs());
        }

        private void AppealsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                if (e.OriginalSource.GetType() == typeof(DataGrid))
                {
                    foreach (dynamic item in e.AddedItems)
                    {
                        selectedTests.Add((BE.Test)bl.GetAllTests(t => t.TestID == item.TestID).First());
                    }
                    foreach (dynamic item in e.RemovedItems)
                    {
                        selectedTests.RemoveAll(t => t.TestID == item.TestID);
                    }

                    if (selectedTests.Count > 1)
                    {
                        this.DesisionButton.IsEnabled = false;
                        this.DesisionButton.ToolTip = "יש לבחור פריט אחד לטיפול";
                    }
                    else if (selectedTests[0].AppealTest.appealStatus != BE.AppealStatus.ממתין)
                    {
                        this.DesisionButton.IsEnabled = false;
                        this.DesisionButton.ToolTip = "כבר טופל";
                    }
                    else
                    {
                        this.DesisionButton.IsEnabled = true;
                        this.DesisionButton.ToolTip = null;
                    }
                }
            }
        }

        private void ApplyFiltering(object sender, RoutedEventArgs e)
        {
            int parsResult;
            int.TryParse(this.SearchTextBox.Text, out parsResult);
            this.AppealsDataGrid.ItemsSource = from item in bl.GetAllTests(t => t.AppealTest != null).Where(t =>
                                                     (this.FromTimeDatePicker.SelectedDate == null || t.AppealTest.RequestTime >= this.FromTimeDatePicker.SelectedDate)
                                    && (this.ToTimeDatePicker.SelectedDate == null || t.AppealTest.RequestTime <= this.ToTimeDatePicker.SelectedDate)
                                    && (this.SearchTextBox.Text == null || t.TestID == parsResult 
                                    || t.TesterID.Contains(this.SearchTextBox.Text) || t.TraineeID.Contains(this.SearchTextBox.Text)) 
                                    && (this.StatusComboBox.SelectedIndex == -1 || t.AppealTest.appealStatus.Equals(this.StatusComboBox.SelectedValue)))
                                               select new { TestID = item.TestID, TraineeID = item.TraineeID, TesterID = item.TesterID,
                                                   RequestTime = item.AppealTest.RequestTime.ToString("dd/MM/yyyy HH:mm"),
                                                   DecisionTime = (item.AppealTest.appealStatus != BE.AppealStatus.ממתין ? item.AppealTest.DecisionTime.ToString("dd/MM/yyyy HH:mm") : "טרם טופל"),
                                                   AppealStatus = item.AppealTest.appealStatus, TraineeNotes = item.AppealTest.TraineeNotes,
                                                   Decision = item.AppealTest.Decision };
        }

        private void AppealsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TestID":
                    e.Column.Header = "מספר סידורי";
                    break;
                case "TraineeID":
                    e.Column.Header = "מספר התלמיד";
                    break;
                case "TesterID":
                    e.Column.Header = "מספר הבוחן";
                    break;
                case "RequestTime":
                    e.Column.Header = "מועד הבקשה";
                    break;
                case "DecisionTime":
                    e.Column.Header = "מועד הטיפול";
                    break;
                case "AppealStatus":
                    e.Column.Header = "סטטוס";
                    break;
                case "TraineeNotes":
                    e.Column.Header = "תוכן הבקשה";
                    break;
                case "Decision":
                    e.Column.Header = "התשובה";
                    break;

                default:
                    break;
            }
        }
    }
}
