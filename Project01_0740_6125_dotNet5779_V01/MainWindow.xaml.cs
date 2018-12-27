using System;
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
    public partial class MainWindow : Window
    {
        BL.IBL bl;
        public List<Trainee> selectedTrainees = new List<Trainee>();
        public List<Tester> selectedTesters = new List<Tester>();
        public List<Test> selectedTests = new List<Test>();
        public MainWindow()
        {
            InitializeComponent();
            bl = BL.Factory.GetInstance();

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
            this.TraineesTabUserControl.UpdateTestResultButton.Visibility = Visibility.Collapsed;
            this.TraineesTabUserControl.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.TraineesTabUserControl.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.TraineesTabUserControl.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            ApplyTraineesFiltering(this, new RoutedEventArgs());


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
            this.TestersTabUserControl.UpdateTestResultButton.Visibility = Visibility.Collapsed;
            this.TestersTabUserControl.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.TestersTabUserControl.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.TestersTabUserControl.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            this.TestersTabUserControl.passedComboBox.Visibility = Visibility.Collapsed;
            this.TestersTabUserControl.passedLable.Visibility = Visibility.Collapsed;
            ApplyTestersFiltering(this, new RoutedEventArgs());


            this.TestsTabUserControl.AddButton.Content = "הוסף טסט";
            this.TestsTabUserControl.DeleteButton.Content = "מחק טסט";
            this.TestsTabUserControl.UpdateButton.Content = "ערוך טסט";
            this.TestsTabUserControl.passedLable.Content = "עבר";
            this.TestsTabUserControl.AddButton.Click += AddTestButton_Click;
            this.TestsTabUserControl.UpdateButton.Click += UpdateTestButton_Click;
            this.TestsTabUserControl.UpdateTestResultButton.Click += UpdateTestResultButton_Click;
            this.TestsTabUserControl.DeleteButton.Click += DeleteTestButton_Click;
            this.TestsTabUserControl.DataGrid.ItemsSource = bl.GetAllTests();
            this.TestsTabUserControl.SearchTextBox.TextChanged += ApplyTestsFiltering;
            this.TestsTabUserControl.passedComboBox.SelectionChanged += ApplyTestsFiltering;
            this.TestsTabUserControl.FromTimeDatePicker.LostFocus += ApplyTestsFiltering;
            this.TestsTabUserControl.ToTimeDatePicker.LostFocus += ApplyTestsFiltering;
            this.TestsTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TestsTabUserControl.ResetFiltersButton.Click += TestsResetFilters;
            this.TestsTabUserControl.DataGrid.SelectionChanged += TestsDataGrid_SelectionChanged;
            this.TestsTabUserControl.gearBoxTypeComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.gearBoxTypeLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.genderComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.genderLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.vehicleComboBox.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.vehicleLabel.Visibility = Visibility.Collapsed;
            this.TestsTabUserControl.ApealsWondow.Visibility = Visibility.Visible;
            this.TestsTabUserControl.ApealsWondow.Click += ApealsWondow_Click;
        }

        private void ApealsWondow_Click(object sender, RoutedEventArgs e)
        {
            new AppeaTests().ShowDialog();
            ApplyTestsFiltering(this, new RoutedEventArgs());
        }


        #region TraineesTab
        private void AddTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            new AddTrainee().ShowDialog();
            ApplyTraineesFiltering(this, new RoutedEventArgs());
        }

        private void UpdateTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            new UpdateTrainee().ShowDialog();
            ApplyTraineesFiltering(this, new RoutedEventArgs());
        }

        private void DeleteTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            string Trainees = "";
            foreach (var item in selectedTrainees)
                Trainees += item.ToString() + "\n\n";
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTrainees.Count + (selectedTrainees.Count == 1 ? " התלמיד שנבחר\n\n" : " התלמידים שנבחרו\n\n") + Trainees;
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה" ,MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedTrainees)
                    bl.RemoveTrainee(item.ID);
                ApplyTraineesFiltering(this, e);
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        private void ApplyTraineesFiltering(object sender, RoutedEventArgs e)
        {
            bool? passed;
            if (this.TraineesTabUserControl.passedComboBox.SelectedItem == null)
                passed = null;
            else
                passed = ((ComboBoxItem)this.TraineesTabUserControl.passedComboBox.SelectedItem).Content.ToString() == "עבר" ? true : false;
            this.TraineesTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTrainees(
                                        this.TraineesTabUserControl.SearchTextBox.Text,
                                        this.TraineesTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                        this.TraineesTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                        this.TraineesTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                        this.TraineesTabUserControl.FromTimeDatePicker.SelectedDate,
                                        this.TraineesTabUserControl.ToTimeDatePicker.SelectedDate,
                                        passed)
                                        select new
                                        {
                                            FirstName = item.FirstName,
                                            LastName = item.LastName,
                                            ID = item.ID,
                                            Gender = item.Gender,
                                            BirthDate = item.BirthDate.ToString("dd/MM/yyyy"),
                                            PhoneNumber = item.PhoneNumber,
                                            Address = item.Address,
                                            MailAddress = item.MailAddress,
                                            Vehicle = item.Vehicle,
                                            gearBoxType = item.gearBoxType,
                                            DrivingSchoolName = item.DrivingSchoolName,
                                            TeacherName = item.TeacherName,
                                            NumOfDrivingLessons = item.NumOfDrivingLessons,
                                            OnlyMyGender = item.OnlyMyGender
                                        }; 
        }

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

        private void TraineesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    foreach (dynamic item in e.AddedItems)
                    {
                        selectedTrainees.Add((Trainee)bl.GetAllTrainees(t=> t.ID == item.ID).First());
                    }
                    foreach (dynamic item in e.RemovedItems)
                    {
                        selectedTrainees.RemoveAll(t => t.ID == item.ID);
                    }
                }
                this.TraineesTabUserControl.UpdateButton.IsEnabled = selectedTrainees.Count == 1;
                this.TraineesTabUserControl.DeleteButton.IsEnabled = selectedTrainees.Count >= 1;
                this.TraineesTabUserControl.DeleteButton.Content = selectedTrainees.Count > 1 ? "מחק תלמידים" : "מחק תלמיד";
            }
        }

        #endregion

        #region TestersTab
        private void AddTesterButton_Click(object sender, RoutedEventArgs e)
        {
            new AddTester().ShowDialog();
            ApplyTestersFiltering(this, new RoutedEventArgs());
        }

        private void UpdateTesterButton_Click(object sender, RoutedEventArgs e)
        {
            new UpdateTester().ShowDialog();
            ApplyTestersFiltering(this, new RoutedEventArgs());
        }

        private void DeleteTesterButton_Click(object sender, RoutedEventArgs e)
        {
            string Testers = "";
            foreach (var item in selectedTesters)
                Testers += item.ToString() + "\n\n";
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTesters.Count + (selectedTesters.Count == 1 ? " הבוחן שנבחר\n\n" : " הבוחנים שנבחרו\n\n") + Testers;
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedTesters)
                    bl.RemoveTester(item.ID);
                ApplyTestersFiltering(this, new RoutedEventArgs());
            }
        }

        private void ApplyTestersFiltering(object sender, RoutedEventArgs e)
        {
            this.TestersTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTesters(
                                        this.TestersTabUserControl.SearchTextBox.Text,
                                        this.TestersTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                        this.TestersTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                        this.TestersTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                        this.TestersTabUserControl.FromTimeDatePicker.SelectedDate,
                                        this.TestersTabUserControl.ToTimeDatePicker.SelectedDate)
                                        select new
                                        {
                                            FirstName = item.FirstName,
                                            LastName = item.LastName,
                                            ID = item.ID,
                                            Gender = item.Gender,
                                            BirthDate = item.BirthDate.ToString("dd/MM/yyyy"),
                                            PhoneNumber = item.PhoneNumber,
                                            Address = item.Address,
                                            MailAddress = item.MailAddress,
                                            Vehicle = item.Vehicle,
                                            gearBoxType = item.gearBoxType,
                                            Experience = item.Experience,
                                            MaxTestsInWeek = item.MaxTestsInWeek,
                                            WorkHours = item.WorkHours,
                                            MaxDistanceInMeters = item.MaxDistanceInMeters
                                        };
        }

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
                    selectedTesters.RemoveAll(t=> t.ID == item.ID);
                }
            this.TestersTabUserControl.UpdateButton.IsEnabled = selectedTesters.Count == 1;
            this.TestersTabUserControl.DeleteButton.IsEnabled = selectedTesters.Count >= 1;
            this.TestersTabUserControl.DeleteButton.Content = selectedTesters.Count > 1 ? "מחק בוחנים" : "מחק בוחן";
            }
        }

        #endregion

        #region TestsTab
        private void AddTestButton_Click(object sender, RoutedEventArgs e)
        {
            new AddTest().ShowDialog();
            ApplyTestsFiltering(this, e);
        }

        private void UpdateTestButton_Click(object sender, RoutedEventArgs e)
        {
            new UpdateTest().ShowDialog();
            ApplyTestsFiltering(this, e);
            this.TestsTabUserControl.DeleteButton.IsEnabled = false;
            this.TestsTabUserControl.UpdateButton.IsEnabled = false;
            this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = false;
        }

        private void UpdateTestResultButton_Click(object sender, RoutedEventArgs e)
        {
            new UpdateTestResult().ShowDialog();
            ApplyTestsFiltering(this, e);
            this.TestsTabUserControl.DeleteButton.IsEnabled = false;
            this.TestsTabUserControl.UpdateButton.IsEnabled = false;
            this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = false;
        }

        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {
            string Teste = "";
            foreach (var item in selectedTests)
                Teste += item.ToString() + "\n\n";
            string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + selectedTests.Count + (selectedTests.Count == 1 ? " הטסטים שנבחר\n\n" : " הטסטים שנבחרו\n\n") + Teste;
            MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedTests)
                    bl.RemoveTest(item.TestID);
                ApplyTestsFiltering(this, e);
                this.TestsTabUserControl.DeleteButton.IsEnabled = false;
                this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = false;
            }
        }

        private void ApplyTestsFiltering(object sender, RoutedEventArgs e)
        {
            this.TestsTabUserControl.DataGrid.ItemsSource = bl.GetAllTests(this.TestsTabUserControl.SearchTextBox.Text,
                                                                           this.TestsTabUserControl.FromTimeDatePicker.SelectedDate,
                                                                           this.TestsTabUserControl.ToTimeDatePicker.SelectedDate,
                                                                           (this.TestsTabUserControl.passedComboBox.SelectedIndex == -1 ? (bool?)null
                                                                                : (this.TestsTabUserControl.passedComboBox.SelectedIndex == 0 ? true : false)));

        }

        private void TestsResetFilters(object sender, RoutedEventArgs e)
        {
            this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.SearchTextBox.Text = "";
            this.TestsTabUserControl.passedComboBox.SelectedItem = null;
            ApplyTestsFiltering(this, e);
        }
        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                if (e.OriginalSource.GetType() == typeof(DataGrid))
                {
                    foreach (Test item in e.AddedItems)
                    {
                        selectedTests.Add(item);
                    }
                    foreach (Test item in e.RemovedItems)
                    {
                        selectedTests.Remove(item);
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
                        this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = true;
                        this.TestsTabUserControl.UpdateTestResultButton.ToolTip = null;
                    }
                    else
                    {
                        this.TestsTabUserControl.UpdateButton.IsEnabled = true;
                        this.TestsTabUserControl.UpdateButton.ToolTip = null;
                        this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = false;
                        this.TestsTabUserControl.UpdateTestResultButton.ToolTip = "לא ניתן לעדכן תוצאות לטסט שעדיין לא בוצע";
                    }
                }
                else
                {
                    this.TestsTabUserControl.UpdateButton.IsEnabled = false;
                    this.TestsTabUserControl.UpdateTestResultButton.IsEnabled = false;
                    this.TestsTabUserControl.UpdateTestResultButton.ToolTip = "יש לבחור פריט אחד לעדכון";
                    this.TestsTabUserControl.UpdateButton.ToolTip = "יש לבחור פריט אחד לעריכה";
                }
            }
        }
        #endregion


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
                case "gearBoxType":
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
                this.blankTabItem.Width  = e.NewSize.Width - 80 * 3 - 65 - 30;
        }
    }
}
