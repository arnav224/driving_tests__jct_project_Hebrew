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
        List<Tester> selectedTester;
        List<Test> selectedTests;
        public MainWindow()
        {
            InitializeComponent();
            bl = BL.Factory.GetInstance();

            this.TraineesTabUserControl.AddButton.Content = "הוספת תלמיד";
            this.TraineesTabUserControl.DeleteButton.Content = "מחיקת תלמיד";
            this.TraineesTabUserControl.UpdateButton.Content = "עדכון תלמיד";
            this.TraineesTabUserControl.AddButton.Click += AddTraineeButton_Click;
            this.TraineesTabUserControl.UpdateButton.Click += UpdateTraineeButton_Click;
            this.TraineesTabUserControl.DeleteButton.Click += DeleteTraineeButton_Click;
            this.TraineesTabUserControl.DataGrid.ItemsSource = bl.GetAllTrainees();
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



            this.TestersTabUserControl.AddButton.Content = "הוספת בוחן";
            this.TestersTabUserControl.DeleteButton.Content = "מחיקת בוחן";
            this.TestersTabUserControl.UpdateButton.Content = "עדכון בוחן";
            this.TestersTabUserControl.AddButton.Click += AddTesterButton_Click;
            this.TestersTabUserControl.UpdateButton.Click += UpdateTesterButton_Click;
            this.TestersTabUserControl.DeleteButton.Click += DeleteTesterButton_Click;
            this.TestersTabUserControl.DataGrid.ItemsSource = bl.GetAllTesters();
            this.TestersTabUserControl.FromTimeDatePicker.LostFocus += ApplyTestersFiltering;
            this.TestersTabUserControl.ToTimeDatePicker.LostFocus += ApplyTestersFiltering;
            this.TestersTabUserControl.SearchTextBox.TextChanged += ApplyTestersFiltering;
            this.TestersTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TestersTabUserControl.ResetFiltersButton.Click += TestersResetFilters;
            this.TestersTabUserControl.DataGrid.SelectionChanged += TestersDataGrid_SelectionChanged;
            this.TestersTabUserControl.UpdateTestResultButton.Visibility = Visibility.Collapsed;

            this.TestsTabUserControl.AddButton.Content = "הוספת טסט";
            this.TestsTabUserControl.DeleteButton.Content = "מחיקת טסט";
            this.TestsTabUserControl.UpdateButton.Content = "עדכון טסט";
            this.TestsTabUserControl.AddButton.Click += AddTestButton_Click;
            this.TestsTabUserControl.UpdateButton.Click += UpdateTestButton_Click;
            this.TestsTabUserControl.UpdateTestResultButton.Click += UpdateTestResultButton_Click;
            this.TestsTabUserControl.DeleteButton.Click += DeleteTestButton_Click;
            this.TestsTabUserControl.DataGrid.ItemsSource = bl.GetAllTests();
            this.TestsTabUserControl.FromTimeDatePicker.LostFocus += ApplyTestsFiltering;
            this.TestsTabUserControl.ToTimeDatePicker.LostFocus += ApplyTestsFiltering;
            this.TestsTabUserControl.SearchTextBox.TextChanged += ApplyTestsFiltering;
            this.TestsTabUserControl.DataGrid.AutoGeneratingColumn += TraineesDataGrid_AutoGeneratingColumn;
            this.TestsTabUserControl.ResetFiltersButton.Click += TestsResetFilters;
            this.TestsTabUserControl.DataGrid.SelectionChanged += TestsDataGrid_SelectionChanged;


            ////@temp
            //this.TestsTabUserControl.UpdateButton.Click += UpdateTestResultButton_Click;
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

            //if (this.TraineesTabUserControl.Update_IdTextBox.Text == "")
            //    return;
            //else if (!bl.GetAllTrainees(t => t.ID == this.TraineesTabUserControl.Update_IdTextBox.Text).Any())
            //    MessageBox.Show(this.TraineesTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא תלמיד",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //else
            //{
            //    new UpdateTrainee().ShowDialog();
            //    ApplyTraineesFiltering(this, new RoutedEventArgs());
            //}
        }

        private void DeleteTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            string Trainees = "";
            foreach (var item in selectedTrainees)
            {
                Trainees += item.ToString() + "\n\n";
            }
            string str = "?אתה בטוח שאתה רוצה למחוק את " + (selectedTrainees.Count == 1 ? "התלמיד שנבחר\n\n" : "התלמידים שנבחרו\n\n") + Trainees;
            MessageBoxResult result = MessageBox.Show(str, "אישור מחיקה",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in selectedTrainees)
                {
                bl.RemoveTrainee(item.ID);
                }
                ApplyTraineesFiltering(this, new RoutedEventArgs());
            }

            {//if (this.TraineesTabUserControl.Update_IdTextBox.Text == "")
             //    return;
             //Trainee trainee = bl.GetAllTrainees(t => t.ID == this.TraineesTabUserControl.Update_IdTextBox.Text).FirstOrDefault();
             //if (trainee == null)
             //{
             //    MessageBox.Show(this.TraineesTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא תלמיד",
             //        MessageBoxButton.OK, MessageBoxImage.Error);
             //    return;
             //}
             //MessageBoxResult result = MessageBox.Show("למחוק את התלמיד " + trainee.FirstName + ' ' + trainee.LastName + ' ' + trainee.ID + " ?",
             //                              "אישור מחיקה",
             //                              MessageBoxButton.YesNo,
             //                              MessageBoxImage.Question);
             //if (result == MessageBoxResult.Yes)
             //{
             //    bl.RemoveTrainee(trainee.ID);
             //    ApplyTraineesFiltering(this, new RoutedEventArgs());
             //}
            }
        }

        private void ApplyTraineesFiltering(object sender, RoutedEventArgs e)
        {
            bool? passed;
            if (this.TraineesTabUserControl.passedComboBox.SelectedItem == null)
                passed = null;
            else
                passed = ((ComboBoxItem)this.TraineesTabUserControl.passedComboBox.SelectedItem).Content.ToString() == "עבר" ? true : false;
            this.TraineesTabUserControl.DataGrid.ItemsSource = null;
            this.TraineesTabUserControl.DataGrid.ItemsSource = bl.GetAllTrainees(
                                        this.TraineesTabUserControl.SearchTextBox.Text,
                                        this.TraineesTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                        this.TraineesTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                        this.TraineesTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                        this.TraineesTabUserControl.FromTimeDatePicker.SelectedDate,
                                        this.TraineesTabUserControl.ToTimeDatePicker.SelectedDate,
                                        passed); 
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
                    foreach (Trainee item in e.AddedItems)
                    {
                        selectedTrainees.Add(item);
                    }
                    foreach (Trainee item in e.RemovedItems)
                    {
                        selectedTrainees.Remove(item);
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
            if (this.TestersTabUserControl.Update_IdTextBox.Text == "")
                return;
            else if (!bl.GetAllTesters(t => t.ID == this.TestersTabUserControl.Update_IdTextBox.Text).Any())
                MessageBox.Show(this.TraineesTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא בוחן",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new UpdateTester().ShowDialog();
                ApplyTestersFiltering(this, new RoutedEventArgs());
            }
        }

        private void DeleteTesterButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TestersTabUserControl.Update_IdTextBox.Text == "")
                return;
            Tester tester = bl.GetAllTesters(t => t.ID == this.TestersTabUserControl.Update_IdTextBox.Text).FirstOrDefault();
            if (tester == null)
            {
                MessageBox.Show(this.TestersTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא בוחן",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (bl.GetAllTests(t=> t.TesterID == tester.ID).Any())
            {
                MessageBox.Show(" לא ניתן להסיר את הבוחן, " + this.TestersTabUserControl.Update_IdTextBox.Text +"  יש לו טסטים רשומים במערכת\nהסר אותם תחילה" , "שגיאה - לא ניתן להסיר",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("למחוק את הבוחן " + tester.FirstName + ' ' + tester.LastName + ' ' + tester.ID + " ?",
                                          "אישור מחיקה",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bl.RemoveTester(tester.ID);
                ApplyTestersFiltering(this, new RoutedEventArgs());
            }
        }

        private void ApplyTestersFiltering(object sender, RoutedEventArgs e)
        {
            DateTime? FromTime = this.TestersTabUserControl.FromTimeDatePicker.SelectedDate;
            DateTime? ToTime = this.TestersTabUserControl.ToTimeDatePicker.SelectedDate;
            string searchString = this.TestersTabUserControl.SearchTextBox.Text;
            this.TestersTabUserControl.DataGrid.ItemsSource = null;
            this.TestersTabUserControl.DataGrid.ItemsSource = bl.GetAllTesters(t =>
                (FromTime == null || t.BirthDate >= FromTime) && (ToTime == null || t.BirthDate <= ToTime)
                && (t.FirstName.Contains(searchString) || t.LastName.Contains(searchString) || (t.FirstName + ' ' + t.LastName).Contains(searchString)
                || t.ID.Contains(searchString) || t.Address.Contains(searchString) || t.MailAddress.Contains(searchString)));
        }

        private void TestersResetFilters(object sender, RoutedEventArgs e)
        {
            this.TestersTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TestersTabUserControl.ToTimeDatePicker.SelectedDate = null;
            this.TestersTabUserControl.SearchTextBox.Text = "";
            ApplyTestersFiltering(this, new RoutedEventArgs());
        }

        private void TestersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    //open the expender
                }
            }
            else
                ;//close the expender
        }

        #endregion

        #region TestsTab
        private void AddTestButton_Click(object sender, RoutedEventArgs e)
        {
            new AddTest().ShowDialog();
            ApplyTestsFiltering(this, new RoutedEventArgs());
        }

        private void UpdateTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TestsTabUserControl.Update_IdTextBox.Text == "")
                return;
            Test test = bl.GetAllTests(t => t.TestID == Convert.ToInt32(this.TestsTabUserControl.Update_IdTextBox.Text)).FirstOrDefault();
            if (test == null)
            MessageBox.Show(this.TestsTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא טסט",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else if (test.Time < DateTime.Now)
                MessageBox.Show(this.TestsTabUserControl.Update_IdTextBox.Text + " מועד הטסט חלף,", "שגיאה - מועד הטסט חלף",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new UpdateTest().ShowDialog();
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        private void UpdateTestResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TestsTabUserControl.Update_IdTextBox.Text == "")
                return;
            Test test = bl.GetAllTests(t => t.TestID == Convert.ToInt32(this.TestsTabUserControl.Update_IdTextBox.Text)).FirstOrDefault();
            if (test == null)
                MessageBox.Show(this.TestsTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא טסט",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            if (test.Time > DateTime.Now)
                MessageBox.Show("לא ניתן לעדכן תוצאות לטסט שעדיין לא התבצע.", "שגיאה - הטסט עדיין לא התבצע",
        MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new UpdateTestResult().ShowDialog();
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TestsTabUserControl.Update_IdTextBox.Text == "")
                return;
            Test test = bl.GetAllTests(t => t.TestID ==Convert.ToInt32(this.TestsTabUserControl.Update_IdTextBox.Text)).FirstOrDefault();
            if (test == null)
            {
                MessageBox.Show(this.TestsTabUserControl.Update_IdTextBox.Text + " לא קיים במערכת המספר,", "שגיאה - לא נמצא טסט",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("למחוק את טסט " + test.TestID +  " ?",
                                          "אישור מחיקה",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bl.RemoveTest(test.TestID);
                ApplyTestsFiltering(this, new RoutedEventArgs());
            }
        }

        private void ApplyTestsFiltering(object sender, RoutedEventArgs e)
        {
            DateTime? FromTime = this.TestsTabUserControl.FromTimeDatePicker.SelectedDate;
            DateTime? ToTime = this.TestsTabUserControl.ToTimeDatePicker.SelectedDate;
            string searchString = this.TestsTabUserControl.SearchTextBox.Text;
            this.TestsTabUserControl.DataGrid.ItemsSource = null;
            int parsResult;
            bool isNumber = int.TryParse(searchString, out parsResult);
            this.TestsTabUserControl.DataGrid.ItemsSource = bl.GetAllTests(t =>
                                                            (FromTime == null || t.Time >= FromTime) && (ToTime == null || t.Time <= ToTime)
                                                            && (searchString == null || t.TestID == parsResult
                                                            || t.TesterID.Contains(searchString) || t.TraineeID.Contains(searchString)
                                                            || t.Address.Contains(searchString)));
        }

        private void TestsResetFilters(object sender, RoutedEventArgs e)
        {
            this.TestsTabUserControl.FromTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.ToTimeDatePicker.SelectedDate = null;
            this.TestsTabUserControl.SearchTextBox.Text = "";
            ApplyTestsFiltering(this, new RoutedEventArgs());
        }
        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    //open the expender
                }
            }
            else
                ;//close the expender
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
