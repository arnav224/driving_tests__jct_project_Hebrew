﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Threading;
using BE;

namespace Project01_0740_6125_dotNet5779_V01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        BL.IBL bl;
        public List<Trainee> selectedTrainees = new List<Trainee>();
        public List<Tester> selectedTesters = new List<Tester>();
        public List<Test> selectedTests = new List<Test>();
        Queue<KeyValuePair<string, DateTime>> notificationsQueue = new Queue<KeyValuePair<string, DateTime>>();
        readonly static int NumOfDisplayItems = 7;
        public dynamic htmlText;
        DateTime timeOfLastNotification;
        public MainWindow()
        {
            bl = BL.Factory.GetInstance();
            InitializeComponent();
            bl.SendTestsRemindersLoop();

            #region TraineesTab
            this.TraineesTabUserControl.AddButton.Content = "הוסף תלמיד";
            this.TraineesTabUserControl.DeleteButton.Content = "מחק תלמיד";
            this.TraineesTabUserControl.UpdateButton.Content = "ערוך תלמיד";
            this.TraineesTabUserControl.AddButton.Click += AddTraineeButton_Click;
            this.TraineesTabUserControl.UpdateButton.Click += UpdateTraineeButton_Click;
            this.TraineesTabUserControl.DeleteButton.Click += DeleteTraineeButton_Click;
            this.TraineesTabUserControl.SearchTextBox.TextChanged += ApplyTraineesFiltering;
            this.TraineesTabUserControl.genderComboBox.SelectionChanged += ApplyTraineesFiltering;
            this.TraineesTabUserControl.gearBoxTypeComboBox.SelectionChanged += ApplyTraineesFiltering;
            this.TraineesTabUserControl.vehicleComboBox.SelectionChanged += ApplyTraineesFiltering;
            this.TraineesTabUserControl.FromTimeDatePicker.LostFocus += ApplyTraineesFiltering;
            this.TraineesTabUserControl.ToTimeDatePicker.LostFocus += ApplyTraineesFiltering;
            this.TraineesTabUserControl.passedComboBox.SelectionChanged += ApplyTraineesFiltering;
            this.TraineesTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TraineesTabUserControl.ResetFiltersButton.Click += TraineesResetFilters;
            this.TraineesTabUserControl.DataGrid.SelectionChanged += TraineesDataGrid_SelectionChanged;
            this.TraineesTabUserControl.DataGrid.MouseDoubleClick += UpdateTraineeButton_Click;
            this.TraineesTabUserControl.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.TraineesTabUserControl.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.TraineesTabUserControl.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            this.TraineesTabUserControl.OptionalButton.Content = "הוסף טסט";
            this.TraineesTabUserControl.OptionalButton.Click += AddTestToTraineeButton_Click;

            ApplyTraineesFiltering(this, new RoutedEventArgs());
            #endregion

            #region TestersTab
            this.TestersTabUserControl.AddButton.Content = "הוסף בוחן";
            this.TestersTabUserControl.DeleteButton.Content = "מחק בוחן";
            this.TestersTabUserControl.UpdateButton.Content = "ערוך בוחן";
            this.TestersTabUserControl.AddButton.Click += AddTesterButton_Click;
            this.TestersTabUserControl.UpdateButton.Click += UpdateTesterButton_Click;
            this.TestersTabUserControl.DeleteButton.Click += DeleteTesterButton_Click;
            this.TestersTabUserControl.SearchTextBox.TextChanged += ApplyTestersFiltering;
            this.TestersTabUserControl.genderComboBox.SelectionChanged += ApplyTestersFiltering;
            this.TestersTabUserControl.gearBoxTypeComboBox.SelectionChanged += ApplyTestersFiltering;
            this.TestersTabUserControl.vehicleComboBox.SelectionChanged += ApplyTestersFiltering;
            this.TestersTabUserControl.FromTimeDatePicker.LostFocus += ApplyTestersFiltering;
            this.TestersTabUserControl.ToTimeDatePicker.LostFocus += ApplyTestersFiltering;
            this.TestersTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TestersTabUserControl.ResetFiltersButton.Click += TestersResetFilters;
            this.TestersTabUserControl.DataGrid.SelectionChanged += TestersDataGrid_SelectionChanged;
            this.TestersTabUserControl.DataGrid.MouseDoubleClick += UpdateTesterButton_Click;
            this.TestersTabUserControl.OptionalButton.Visibility = Visibility.Collapsed;
            this.TestersTabUserControl.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.TestersTabUserControl.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.TestersTabUserControl.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            this.TestersTabUserControl.passedComboBox.Visibility = Visibility.Collapsed;
            this.TestersTabUserControl.passedLable.Visibility = Visibility.Collapsed;
            ApplyTestersFiltering(this, new RoutedEventArgs());
            #endregion

            #region TestsTab
            this.TestsTabUserControl.AddButton.Content = "הוסף טסט";
            this.TestsTabUserControl.DeleteButton.Content = "מחק טסט";
            this.TestsTabUserControl.UpdateButton.Content = "ערוך טסט";
            this.TestsTabUserControl.passedLable.Content = "עבר";
            this.TestsTabUserControl.AddButton.Click += AddTestButton_Click;
            this.TestsTabUserControl.UpdateButton.Click += UpdateTestButton_Click;
            this.TestsTabUserControl.OptionalButton.Click += UpdateTestResultButton_Click;
            this.TestsTabUserControl.DeleteButton.Click += DeleteTestButton_Click;
            this.TestsTabUserControl.SearchTextBox.TextChanged += ApplyTestsFiltering;
            this.TestsTabUserControl.passedComboBox.SelectionChanged += ApplyTestsFiltering;
            this.TestsTabUserControl.FromTimeDatePicker.LostFocus += FromAndToTimeDatePicker_LostFocus; ;
            this.TestsTabUserControl.ToTimeDatePicker.LostFocus += FromAndToTimeDatePicker_LostFocus;
            this.TestsTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TestsTabUserControl.ResetFiltersButton.Click += TestsResetFilters;
            this.TestsTabUserControl.DataGrid.SelectionChanged += TestsDataGrid_SelectionChanged;
            this.TestsTabUserControl.DataGrid.MouseDoubleClick += TestsDataGrid_MouseDoubleClick;
            this.TestsTabUserControl.gearBoxTypeComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.gearBoxTypeLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.genderComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.genderLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.vehicleComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.vehicleLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.ApealsWondow.Visibility = Visibility.Visible;
            this.TestsTabUserControl.AppealButton.Visibility = Visibility.Visible;
            this.TestsTabUserControl.AppealButton.Click += AppealButton_Click;
            this.TestsTabUserControl.ApealsWondow.Click += ApealsWondow_Click;
            this.TestsTabUserControl.SendMailButton.Click += TestSendMailButton_Click;
            this.TestsTabUserControl.SendMailButton.Visibility = Visibility.Visible;
            this.TestsTabUserControl.timeLabel.Visibility = Visibility.Visible;
            this.TestsTabUserControl.timeComboBox.Visibility = Visibility.Visible;
            this.TestsTabUserControl.timeComboBox.SelectionChanged += ApplyTestsFiltering;
            this.ApplyTestsFiltering(this, new RoutedEventArgs());
            #endregion

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => { Thread.Sleep(2000); };
            worker.RunWorkerCompleted += (s, e) =>
            {
                if (Configuration.GoogleMapsApiKey == "" || Configuration.EmailServerPasword == "" || Configuration.SenderEmailAddress == "")
                {
                    var result = MessageBox.Show(((Configuration.EmailServerPasword == "" || Configuration.SenderEmailAddress == "") ? ".כדי לשלוח מיילים עליך להכניס הגדרות מייל\n" : "")
                        + ((Configuration.GoogleMapsApiKey == "") ? "כדי להשתמש במפות גוגל (השלמה אוטומטית, חישוב מרחקים)\n.Developer key עליך להכניס" : "")
                        + "\n\n?לעבור לטאב ההגדרות"
                        , "חסרות הגדרות אופציונאליות", MessageBoxButton.YesNo,
                                    MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    if (result == MessageBoxResult.Yes)
                        this.TabControl.SelectedIndex = 4;
                }

            };
            worker.RunWorkerAsync();
        }


        #region TraineesTab
        /// <summary>
        /// Add Trainee Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new AddTrainee().ShowDialog() == true)
                    AddNotification("תלמיד נוסף בהצלחה");
                ApplyTraineesFiltering(this, new RoutedEventArgs());
            }
        }
        
        /// <summary>
        /// Update Trainee ButtonClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new UpdateTrainee().ShowDialog() == true)
                    AddNotification("התלמיד " + selectedTrainees[0].FirstName + ' ' + selectedTrainees[0].LastName + " עודכן בהצלחה");
                ApplyTraineesFiltering(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Add Test To the selected Trainee Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestToTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new AddTest(selectedTrainees[0].ID).ShowDialog() == true)
                    AddNotification("הטסט נוסף בהצלחה לתלמיד " + selectedTrainees[0].FirstName + ' ' + selectedTrainees[0].LastName);
                ApplyTestsFiltering(this, e);
            }
        }

        /// <summary>
        /// Delete Trainee Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            int SumItemsToDisplay = NumOfDisplayItems;
            string Trainees = "";
            // print the trainees Designated for deletion:
            foreach (var TraineeItem in selectedTrainees)
            {
                Trainees += TraineeItem.ToString();
                IEnumerable<Test> testsOfOne = bl.GetAllTests(t => t.TraineeID == TraineeItem.ID);
                if (testsOfOne.Any())
                {
                    Trainees += " תלמיד זה רשום " + (testsOfOne.Count() > 1 ? "לטסטים בתאריכים הבאים: \n" : "לטסט בתאריך: \n");
                    foreach (var TestItem in testsOfOne)
                    {
                        Trainees += TestItem.Time.ToString("dd/MM/yyyy") + ' ';
                    }
                }
                Trainees += "\n\n";
                if (--SumItemsToDisplay == 0)
                    break;
            }
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTrainees.Count + (selectedTrainees.Count == 1 ? " התלמיד שנבחר\n\n" : " התלמידים שנבחרו\n\n") + Trainees
                 + (selectedTrainees.Count > NumOfDisplayItems ? "רשימה חלקית.\n\n" : "");
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                // Remove the Trainee and his tests:
                foreach (var TraineeItem in selectedTrainees)
                {
                    List<Test> testsOfOne = bl.GetAllTests(t => t.TraineeID == TraineeItem.ID).ToList();
                    foreach (var TestItem in testsOfOne)
                    {
                        bl.RemoveTest(TestItem.TestID);
                    }
                    bl.RemoveTrainee(TraineeItem.ID);
                }
                // Notification:
                if (selectedTrainees.Count == 1)
                    AddNotification("התלמיד " + selectedTrainees[0].FirstName + ' ' + selectedTrainees[0].LastName + " נמחק בהצלחה");
                else
                    AddNotification(selectedTrainees.Count.ToString() + " תלמידים נמחקו בהצלחה");
                ApplyTraineesFiltering(this, e);
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Apply Trainees Filtering - update the Trainees data Grid by the selected Filtering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyTraineesFiltering(object sender, RoutedEventArgs e)
        {
            bool? passed;
            if (this.TraineesTabUserControl.passedComboBox.SelectedItem == null)
                passed = null;
            else
                passed = ((ComboBoxItem)this.TraineesTabUserControl.passedComboBox.SelectedItem).Content.ToString() == "עבר" ? true : false;
            try
            {
                this.TraineesTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTrainees(
                                            this.TraineesTabUserControl.SearchTextBox.Text,
                                            this.TraineesTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                            this.TraineesTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                            this.TraineesTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                            this.TraineesTabUserControl.FromTimeDatePicker.SelectedDate,
                                            this.TraineesTabUserControl.ToTimeDatePicker.SelectedDate,
                                            passed)
                                                                   orderby item.LastName, item.FirstName
                                                                   select new
                                                                   {
                                                                       item.FirstName,
                                                                       item.LastName,
                                                                       item.ID,
                                                                       item.Gender,
                                                                       BirthDate = item.BirthDate.ToShortDateString(),
                                                                       item.PhoneNumber,
                                                                       item.Address,
                                                                       item.MailAddress,
                                                                       item.Vehicle,
                                                                       item.GearBoxType,
                                                                       item.DrivingSchoolName,
                                                                       item.TeacherName,
                                                                       item.NumOfDrivingLessons,
                                                                       item.OnlyMyGender,
                                                                       עבר = bl.PassedTest(item.ID) ? "עבר" : ""
                                                                   };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
        }

        /// <summary>
        /// Trainees Reset Filters and show all Trainees
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineesResetFilters(object sender, RoutedEventArgs e)
        {
            this.TraineesTabUserControl.SearchTextBox.Text = "";
            this.TraineesTabUserControl.SearchTextBox.Text = null;
            this.TraineesTabUserControl.genderComboBox.SelectedItem = null;
            this.TraineesTabUserControl.gearBoxTypeComboBox.SelectedItem = null;
            this.TraineesTabUserControl.vehicleComboBox.SelectedItem = null;
            this.TraineesTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TraineesTabUserControl.ToTimeDatePicker.SelectedDate = null;
            this.TraineesTabUserControl.passedComboBox.SelectedItem = null;
            ApplyTraineesFiltering(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Trainees DataGrid SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    foreach (dynamic item in e.AddedItems)
                    {
                        selectedTrainees.Add((Trainee)bl.GetAllTrainees(t => t.ID == item.ID).First());
                    }
                    foreach (dynamic item in e.RemovedItems)
                    {
                        selectedTrainees.RemoveAll(t => t.ID == item.ID);
                    }
                }

                this.TraineesTabUserControl.UpdateButton.IsEnabled = this.TraineesTabUserControl.OptionalButton.IsEnabled = selectedTrainees.Count == 1;
                this.TraineesTabUserControl.UpdateButton.ToolTip = this.TraineesTabUserControl.OptionalButton.ToolTip = selectedTrainees.Count == 1 ? null : "יש לבחור תלמיד אחד";
                this.TraineesTabUserControl.DeleteButton.IsEnabled = selectedTrainees.Count >= 1;
                this.TraineesTabUserControl.DeleteButton.Content = selectedTrainees.Count > 1 ? "מחק תלמידים" : "מחק תלמיד";
            }
        }

        #endregion

        #region TestersTab
        /// <summary>
        /// Add Tester Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTesterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new AddTester().ShowDialog() == true)
                    AddNotification("נוסף בוחן");
            }
        }

        /// <summary>
        /// Update Tester Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTesterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new UpdateTester().ShowDialog() == true)
                    AddNotification("הבוחן " + selectedTesters[0].FirstName + ' ' + selectedTesters[0].LastName + " עודכן בהצלחה");
                ApplyTestersFiltering(this, new RoutedEventArgs());
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Delete Tester Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTesterButton_Click(object sender, RoutedEventArgs e)
        {
            int SumItemsToDisplay = NumOfDisplayItems;
            string Testers = "";
            // print the Testers Designated for deletion:
            foreach (var TesterItem in selectedTesters)
            {
                Testers += TesterItem.ToString() + "\n";
                IEnumerable<Test> testsOfOne = bl.GetAllTests(t => t.TesterID == TesterItem.ID && t.Time > DateTime.Now);
                if (testsOfOne.Any())
                {
                    Testers += " בוחן זה רשום " + (testsOfOne.Count() > 1 ? "לטסטים בתאריכים הבאים: \n" : "לטסט בתאריך: \n");
                    foreach (var TestItem in testsOfOne)
                    {
                        Testers += TestItem.Time.ToString("dd/MM/yyyy") + ' ';
                    }
                }
                Testers += "\n\n";
                if (--SumItemsToDisplay == 0)
                    break;
            }
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTesters.Count + (selectedTesters.Count == 1 ? " הבוחן שנבחר\n\n" : " הבוחנים שנבחרו\n\n") + Testers
                 + (selectedTesters.Count > NumOfDisplayItems ? "רשימה חלקית.\n\n" : "");
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                // Remove the Tester and his tests:
                foreach (var TesterItem in selectedTesters)
                {
                    List<Test> testsOfOne = bl.GetAllTests(t => t.TesterID == TesterItem.ID && t.Time > DateTime.Now).ToList();
                    foreach (var TestItem in testsOfOne)
                    {
                        bl.RemoveTest(TestItem.TestID);
                    }
                    bl.RemoveTester(TesterItem.ID);
                }
                // Notification:
                if (selectedTesters.Count == 1)
                    AddNotification("הבוחן " + selectedTesters[0].FirstName + ' ' + selectedTesters[0].LastName + " נמחק בהצלחה");
                else
                    AddNotification(selectedTesters.Count.ToString() + " בוחנים נמחקו בהצלחה");
                ApplyTestersFiltering(this, new RoutedEventArgs());
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Apply Testers Filtering - update the Testers Data Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyTestersFiltering(object sender, RoutedEventArgs e)
        {
            this.TestersTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTesters(
                                        this.TestersTabUserControl.SearchTextBox.Text,
                                        this.TestersTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                        this.TestersTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                        this.TestersTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                        this.TestersTabUserControl.FromTimeDatePicker.SelectedDate,
                                        this.TestersTabUserControl.ToTimeDatePicker.SelectedDate)
                                                              orderby item.FirstName, item.LastName
                                                              select new
                                                              {
                                                                  item.FirstName,
                                                                  item.LastName,
                                                                  item.ID,
                                                                  item.Gender,
                                                                  BirthDate = item.BirthDate.ToShortDateString(),
                                                                  item.PhoneNumber,
                                                                  item.Address,
                                                                  item.MailAddress,
                                                                  item.Vehicle,
                                                                  item.GearBoxType,
                                                                  item.Experience,
                                                                  item.MaxTestsInWeek,
                                                                  item.WorkHours,
                                                                  item.MaxDistanceInMeters
                                                              };
        }

        /// <summary>
        /// Testers Reset Filters and show all Testers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestersResetFilters(object sender, RoutedEventArgs e)
        {
            this.TestersTabUserControl.SearchTextBox.Text = "";
            this.TestersTabUserControl.SearchTextBox.Text = null;
            this.TestersTabUserControl.genderComboBox.SelectedItem = null;
            this.TestersTabUserControl.gearBoxTypeComboBox.SelectedItem = null;
            this.TestersTabUserControl.vehicleComboBox.SelectedItem = null;
            this.TestersTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TestersTabUserControl.ToTimeDatePicker.SelectedDate = null;
            ApplyTestersFiltering(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Testers DataGrid SelectionChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Type type = e.OriginalSource.GetType();
            if (type == typeof(DataGrid))
            {
                foreach (dynamic item in e.AddedItems)
                {
                    selectedTesters.Add(bl.GetAllTesters(t => t.ID == item.ID).First());
                }
                foreach (dynamic item in e.RemovedItems)
                {
                    selectedTesters.RemoveAll(t => t.ID == item.ID);
                }
                this.TestersTabUserControl.UpdateButton.IsEnabled = selectedTesters.Count == 1;
                this.TestersTabUserControl.DeleteButton.IsEnabled = selectedTesters.Count >= 1;
                this.TestersTabUserControl.DeleteButton.Content = selectedTesters.Count > 1 ? "מחק בוחנים" : "מחק בוחן";
            }
        }
        #endregion

        #region TestsTab
        /// <summary>
        /// Appeal Button_Click - add Appeal request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppealButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AppealRequest().ShowDialog() == true)
                AddNotification("הערעור הוגש בהצלחה");
            ApplyTestsFiltering(this, new RoutedEventArgs());
            this.TestsTabUserControl.AppealButton.IsEnabled = false;
        }

        /// <summary>
        /// open Apeals Wondow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApealsWondow_Click(object sender, RoutedEventArgs e)
        {
            new AppealTests().ShowDialog();
            ApplyTestsFiltering(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Add Test Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new AddTest().ShowDialog() == true)
                    AddNotification("טסט נוסף");
                ApplyTestsFiltering(this, e);
            }
        }

        /// <summary>
        /// Update Test Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new UpdateTest().ShowDialog() == true)
                    AddNotification("טסט עודכן");
                ApplyTestsFiltering(this, e);
                this.TestsTabUserControl.DeleteButton.IsEnabled = false;
                this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                this.TestsTabUserControl.OptionalButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Update Test Mouse Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.TestsTabUserControl.UpdateButton.IsEnabled == true)
                UpdateTestButton_Click(sender, e);
        }

        /// <summary>
        /// Update Test Result Button_Click - the tester report about the test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTestResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (new UpdateTestResult().ShowDialog() == true)
            {
                AddNotification("תוצאות הטסט עודכנו");
                bl.SendEmail(selectedTests[0].TestID, EmailType.TestUpdateResults);
            }
            ApplyTestsFiltering(this, e);
            this.TestsTabUserControl.DeleteButton.IsEnabled = false;
            this.TestsTabUserControl.UpdateButton.IsEnabled = false;
            this.TestsTabUserControl.OptionalButton.IsEnabled = false;
        }

        /// <summary>
        /// Delete Test Button_Click - Only future tests can be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {
            int SumItemsToDisplay = NumOfDisplayItems;
            string Teste = "";
            foreach (var item in selectedTests)
            {
                Teste += item.ToString() + "\n\n";
                if (--SumItemsToDisplay == 0)
                    break;
            }
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTests.Count + (selectedTests.Count == 1 ? " הטסטים שנבחר\n\n" : " הטסטים שנבחרו\n\n") + Teste
                + (selectedTests.Count > NumOfDisplayItems ? "רשימה חלקית.\n\n" : "") + " הודעה במייל תישלח לתלמיד על הביטול";
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedTests)
                {
                    bl.RemoveTest(item.TestID);
                }
                if (selectedTests.Count == 1)
                    AddNotification("טסט נמחק");
                else
                    AddNotification(selectedTests.Count.ToString() + " טסטים נמחקו");

                ApplyTestsFiltering(this, e);
                this.TestsTabUserControl.DeleteButton.IsEnabled = false;
                this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                this.TestsTabUserControl.OptionalButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// From Time DatePicker and To Time DatePicker LostFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromAndToTimeDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            this.TestsTabUserControl.timeComboBox.SelectedIndex = -1;
            ApplyTestsFiltering(this, e);
        }

        /// <summary>
        /// Apply Tests Filtering - update the Tests Data Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyTestsFiltering(object sender, RoutedEventArgs e)
        {
            if (this.TestsTabUserControl.timeComboBox.SelectedIndex != -1)
            {
                DateTime now = DateTime.Now;
                switch (this.TestsTabUserControl.timeComboBox.SelectedIndex)
                {
                    case 0: //this week
                        this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = now.AddDays(-(int)now.DayOfWeek).AddHours(-now.Hour).AddMinutes(-now.Minute);
                        this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = now.AddDays((int)DayOfWeek.Saturday - (int)now.DayOfWeek);
                        break;
                    case 1: //next week
                        this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = now.AddDays(7 - (int)now.DayOfWeek).AddHours(-now.Hour).AddMinutes(-now.Minute);
                        this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = now.AddDays(7 + (int)DayOfWeek.Saturday - (int)now.DayOfWeek);
                        break;
                    case 2: //this month
                        this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = new DateTime(now.Year, now.Month, 1);
                        this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = new DateTime(now.Year, (now.Month + 1) % 12, 1).AddDays(-1);
                        break;
                    case 3: //next month
                        this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = new DateTime(now.Year, (now.Month + 1) % 12, 1);
                        this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = new DateTime(now.Year, (now.Month + 2) % 12, 1).AddDays(-1);
                        break;
                    default:
                        break;
                }
            }
            this.TestsTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTests(this.TestsTabUserControl.SearchTextBox.Text,
                                                            this.TestsTabUserControl.FromTimeDatePicker.SelectedDate,
                                                            this.TestsTabUserControl.ToTimeDatePicker.SelectedDate,
                                                            (this.TestsTabUserControl.passedComboBox.SelectedIndex == -1 ? (bool?)null
                                                                 : (this.TestsTabUserControl.passedComboBox.SelectedIndex == 0 ? true : false)))
                                                            orderby item.Time
                                                            select new
                                                            {
                                                                item.TestID,
                                                                item.TraineeID,
                                                                item.TesterID,
                                                                Time = item.Time.ToString("dd/MM/yyyy HH:mm"),
                                                                item.Address,
                                                                Passed = item.Passed == null ? "" : (item.Passed == true ? "עבר" : "נכשל"),
                                                                item.TesterNotes,
                                                                AppealTest = item.AppealTest == null ? "" : item.AppealTest.appealStatus.ToString(),
                                                            };
        }

        /// <summary>
        /// Tests Reset Filters and show all Tests
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestsResetFilters(object sender, RoutedEventArgs e)
        {
            this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.SearchTextBox.Text = "";
            this.TestsTabUserControl.passedComboBox.SelectedItem = null;
            this.TestsTabUserControl.timeComboBox.SelectedIndex = -1;
            ApplyTestsFiltering(this, e);
        }

        /// <summary>
        /// Tests DataGrid SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TestsTabUserControl.SendMailButton.IsEnabled = false;
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
                }
                bool ExistOldTest = selectedTests.Any(t => t.Time < DateTime.Now);
                this.TestsTabUserControl.DeleteButton.IsEnabled = selectedTests.Count >= 1 && !ExistOldTest;
                if (ExistOldTest)
                    this.TestsTabUserControl.DeleteButton.ToolTip = "לא ניתן למחוק טסט שזמנו עבר";
                else
                    this.TestsTabUserControl.DeleteButton.ToolTip = null;
                this.TestsTabUserControl.DeleteButton.Content = selectedTests.Count > 1 ? "מחק טסטים" : "מחק טסט";
                if (selectedTests.Count == 1)
                {
                    if (selectedTests[0].Time < DateTime.Now)
                    {
                        this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                        this.TestsTabUserControl.UpdateButton.ToolTip = "לא ניתן לערוך פרטים לטסט שכבר נעשה";
                        this.TestsTabUserControl.OptionalButton.IsEnabled = selectedTests[0].AppealTest == null;
                        this.TestsTabUserControl.OptionalButton.ToolTip = null;
                        if (selectedTests[0].Passed == null)
                            this.TestsTabUserControl.AppealButton.ToolTip = "לא התקבלו תוצאות לטסט";
                        else if (selectedTests[0].Passed == true)
                            this.TestsTabUserControl.AppealButton.ToolTip = "התוצאה - עבר";
                        else if (selectedTests[0].AppealTest != null)
                            this.TestsTabUserControl.AppealButton.ToolTip = "לטסט זה כבר הוגש ערעור";
                        else
                        {
                            this.TestsTabUserControl.AppealButton.IsEnabled = true;
                            this.TestsTabUserControl.AppealButton.ToolTip = null;
                        }
                    }
                    else
                    {
                        this.TestsTabUserControl.SendMailButton.IsEnabled = true;
                        this.TestsTabUserControl.AppealButton.IsEnabled = false;
                        this.TestsTabUserControl.UpdateButton.IsEnabled = true;
                        this.TestsTabUserControl.UpdateButton.ToolTip = null;
                        this.TestsTabUserControl.OptionalButton.IsEnabled = false;
                        this.TestsTabUserControl.OptionalButton.ToolTip = "לא ניתן לעדכן תוצאות לטסט שעדיין לא בוצע";
                        this.TestsTabUserControl.AppealButton.ToolTip = "טרם התבצע הטסט";
                    }
                }
                else
                {
                    this.TestsTabUserControl.AppealButton.IsEnabled = false;
                    this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                    this.TestsTabUserControl.OptionalButton.IsEnabled = false;
                    this.TestsTabUserControl.OptionalButton.ToolTip = "יש לבחור פריט אחד לעדכון";
                    this.TestsTabUserControl.UpdateButton.ToolTip = "יש לבחור פריט אחד לעריכה";
                    this.TestsTabUserControl.AppealButton.ToolTip = "יש לבחור פריט אחד לערעור";
                }
            }
        }

        /// <summary>
        /// Test Send Mail Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestSendMailButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            else
            {
                if (new SendingEmail(bl.GetAllTests(t => t.TestID == selectedTests[0].TestID).First()).ShowDialog() == true)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (se, args) =>
                    {
                        try
                        {
                            args.Result = false;
                            string mailAddress = bl.GetAllTrainees(t => t.ID == selectedTests[0].TraineeID).First().MailAddress;
                            if (BE.Tools.SendingEmail(mailAddress, "מועד הטסט שלך מתקרב", htmlText))
                            {
                                args.Result = true;
                                bl.UpdateEmailSendingTime(selectedTests[0].TestID, null, DateTime.Now);
                            }
                        }
                        catch (Exception)
                        {
                            args.Result = false;
                        }
                    };
                    worker.RunWorkerCompleted += (s, arg) =>
                    {
                        AddNotification((bool)arg.Result == true ? "המייל נשלח בהצלחה" : "המייל לא נשלח");
                    };
                    worker.RunWorkerAsync();
                }
            }
        }
        #endregion


        #region Notifications
        /// <summary>
        /// Add Notification 
        /// </summary>
        /// <param name="messege"></param>
        private void AddNotification(string messege)
        {
            KeyValuePair<string, DateTime> pair = new KeyValuePair<string, DateTime>(messege, DateTime.Now);
            if (notificationsQueue.Count >= 4)
                notificationsQueue.Dequeue();
            notificationsQueue.Enqueue(pair);
            RefreshNotification();
            timeOfLastNotification = DateTime.Now;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e)=> { Thread.Sleep(10000); e.Result = pair; };
            worker.RunWorkerCompleted += (s, arg) =>
            {
                RemoveNotification(pair);
            };
            worker.RunWorkerAsync(argument: messege);
        }

        /// <summary>
        /// Remove Notification
        /// </summary>
        /// <param name="pair"></param>
        private void RemoveNotification(KeyValuePair<string, DateTime> pair)
        {
            notificationsQueue = new Queue<KeyValuePair<string, DateTime>>(notificationsQueue.Where(p => !p.Equals(pair))); 
            RefreshNotification();
        }

        /// <summary>
        /// Refresh Notification
        /// </summary>
        private void RefreshNotification()
        {
            NotificationRow0StackPanel.Visibility = NotificationRow1StackPanel.Visibility =
                NotificationRow2StackPanel.Visibility = NotificationRow3StackPanel.Visibility = Visibility.Collapsed;
            if (notificationsQueue.Count >= 1)
            {
                NotificationsRow3Label.Content = notificationsQueue.ElementAt(0).Key;
                NotificationsRow3DatePicker.SelectedDate = notificationsQueue.ElementAt(0).Value;
                NotificationRow3StackPanel.Visibility = Visibility.Visible;
            }
            if (notificationsQueue.Count >= 2)
            {
                NotificationsRow2Label.Content = notificationsQueue.ElementAt(1).Key;
                NotificationsRow2DatePicker.SelectedDate = notificationsQueue.ElementAt(1).Value;
                NotificationRow2StackPanel.Visibility = Visibility.Visible;
            }
            if (notificationsQueue.Count >= 3)
            {
                NotificationsRow1Label.Content = notificationsQueue.ElementAt(2).Key;
                NotificationsRow1DatePicker.SelectedDate = notificationsQueue.ElementAt(2).Value;
                NotificationRow1StackPanel.Visibility = Visibility.Visible;
            }
            if (notificationsQueue.Count >= 4)
            {
                NotificationsRow0Label.Content = notificationsQueue.ElementAt(3).Key;
                NotificationsRow0DatePicker.SelectedDate = notificationsQueue.ElementAt(3).Value;
                NotificationRow0StackPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Notification StackPanel MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationStackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //todo 
            //try
            //{
            //    dynamic parent = e.Source;
            //    while (parent.GetType() != typeof(Viewbox) && parent.GetType() != typeof(StackPanel))
            //        parent = parent.Parent;
            //    if (parent.GetType() == typeof(Viewbox))    //if the 'X' icon is pressed, remove the notification.
            //    {
            //        StackPanel stackPanel = parent.Parent as StackPanel;
            //        Label label = (from dynamic item in stackPanel.Children where item is Label select item as Label).First();
            //        notificationsQueue = new Queue<string>(notificationsQueue.Where(s => s != label.Content.ToString()));
            //        RefreshNotification();
            //    }
            //}
            //catch (Exception) { }

            try
            {
                dynamic parent = e.Source;
                while (parent.GetType() != typeof(Viewbox) && parent.GetType() != typeof(Canvas) && parent.GetType() != typeof(StackPanel))
                    parent = parent.Parent;
                if (parent.GetType() == typeof(Viewbox) || parent.GetType() == typeof(Canvas))
                {
                    while (parent.GetType() != typeof(StackPanel))
                        parent = parent.Parent;
                    StackPanel stackPanel = parent as StackPanel;
                    Label label = (from dynamic item in stackPanel.Children where item is Label select item as Label).First();
                    RefreshNotification();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Notification StackPanel MouseEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationStackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                (e.Source as StackPanel).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF6596F5" /*"#FF5BA5CE"*/));
                (e.Source as StackPanel).Opacity = 1;
                (from dynamic item in (e.Source as StackPanel).Children where item is Viewbox select item as Viewbox).First().Opacity = 0.75;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Notification StackPanel MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationStackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                (e.Source as StackPanel).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCCE4F1"));
                (e.Source as StackPanel).Opacity = 0.8;
                (from dynamic item in (e.Source as StackPanel).Children where item is Viewbox select item as Viewbox).First().Opacity = 0.0;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Viewbox MouseUp - Remove Notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewbox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                dynamic parent = e.Source;
                while (parent.GetType() != typeof(StackPanel))
                    parent = parent.Parent;
                string messege = (string)(from dynamic item in (parent as StackPanel).Children where item is Label select item as Label).First().Content;
                DateTime time = (DateTime)(from dynamic item in (parent as StackPanel).Children where item is DatePicker select item as DatePicker).First().SelectedDate;
                RemoveNotification(new KeyValuePair<string, DateTime>(messege, time));
                RefreshNotification();
            }
            catch (Exception) { }
        }
        #endregion

        /// <summary>
        /// Trainees DataGrid AutoGeneratingColumn - give Neme to columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineesDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ID":
                    e.Column.Header = "תעודת זהות";
                    break;
                case "FirstName":
                    e.Column.Header = "שם פרטי";
                    break;
                case "LastName":
                    e.Column.Header = "שם משפחה";
                    break;
                case "BirthDate":
                    e.Column.Header = "תאריך לידה";
                    break;
                case "Gender":
                    e.Column.Header = "מין";
                    break;
                case "PhoneNumber":
                    e.Column.Header = "מספר טלפון";
                    break;
                case "MailAddress":
                    e.Column.Header = "כתובת מייל";
                    break;
                case "Address":
                    e.Column.Header = "כתובת";
                    break;
                case "Vehicle":
                    e.Column.Header = "סוג רשיון";
                    break;
                case "GearBoxType":
                    e.Column.Header = "תיבת\nהילוכים";
                    break;
                case "DrivingSchoolName":
                    e.Column.Header = "שם\nבית הספר";
                    break;
                case "TeacherName":
                    e.Column.Header = "שם\nהמורה";
                    break;
                case "NumOfDrivingLessons":
                    e.Column.Header = "מספר\nשיעורים";
                    break;
                case "Experience":
                    e.Column.Header = "שנות נסיון";
                    break;
                case "MaxTestsInWeek":
                    e.Column.Header = "מסקסימום\nטסטים לשבוע";
                    break;
                case "MaxDistanceInMeters":
                    e.Column.Header = "מרחק\nמקסימלי";
                    break;
                case "TestID":
                    e.Column.Header = "מספר טסט";
                    break;
                case "TesterID":
                    e.Column.Header = "תעודת זהות\nבוחן";
                    break;
                case "TraineeID":
                    e.Column.Header = "תעודת זהות\nתלמיד";
                    break;
                case "Passed":
                    e.Column.Header = "עבר";
                    break;
                case "WorkHours":
                    e.Cancel = true;
                    break;
                case "Time":
                    e.Column.Header = "זמן";
                    break;
                case "TesterNotes":
                    e.Column.Header = "הערות הבוחן";
                    break;
                case "AppealTest":
                    e.Column.Header = "ערעור";
                    break;
                case "Indices":
                    e.Cancel = true;
                    break;
                case "RemeinderEmailSent":
                    e.Cancel = true;
                    break;
                case "SummaryEmailSent":
                    e.Cancel = true;
                    break;
                case "OnlyMyGender":
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Window SizeChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
                this.blankTabItem.Width = e.NewSize.Width - 115 * 3 - 70 - 30;
        }

        /// <summary>
        /// Toggle Switch Checked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            bl.SendTestsRemindersLoop();
        }

        /// <summary>
        /// Password Box LostFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            BE.Configuration.EmailServerPasword = this.PasswordBox.Password;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }
    }
} 