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
using BE;

namespace Project01_0740_6125_dotNet5779_V01
{
    /// <summary>
    /// Interaction logic for UpdateTester.xaml
    /// </summary>
    public partial class UpdateTester
    {
        BL.IBL bl = BL.Factory.GetInstance();
        List<string> errorMessages = new List<string>();
        List<string> timeErrorMessages = new List<string>();
        BE.Tester tester;
        List<BE.Test> TestsToRemove1 = new List<Test>();
        List<BE.Test> TestsToRemove2;
        List<BE.Test> TestsToRemain;
        List<BE.TimePeriod> timePeriodsToRemove = new List<BE.TimePeriod>();

        /// <summary>
        /// UpdateTester ctor
        /// </summary>
        public UpdateTester()
        {
            try
            {
            tester = ((MainWindow)Application.Current.MainWindow).selectedTesters[0];
            }
            catch (Exception)
            {
                MessageBox.Show("לא נבחר בוחן");
                this.Close();
            }
            InitializeComponent();
            this.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            this.DataContext = tester;
            this.WorkHoursGrid.DataContext = this;
            this.DayComboBox.ItemsSource = Enum.GetValues(typeof(BE.WeekDays));
            List<string> worsHours = (from item in tester.WorkHours select item.ToString()).ToList();
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
            this.addressPicker.Address = tester.Address;
        }

        /// <summary>
        /// Button Click to save the tester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestsToRemain = bl.GetAllTests(test => test.Time > DateTime.Now && tester.WorkHours.Any(t => tester.ID == test.TesterID
                              && t.Start.Days == (int)test.Time.DayOfWeek
                              && t.Start.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) <= test.Time.TimeOfDay
                              && t.End.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) >= test.Time.TimeOfDay + BE.Configuration.TestTimeSpan)).ToList();

            foreach (var TestItem in TestsToRemain)
            {
                if (TestsToRemove1.Any(t => t.ToString() == TestItem.ToString()))
                    TestsToRemove1.RemoveAll(t=> t.TestID == TestItem.TestID);
            }
            foreach (var TestItem in TestsToRemove1)
            {
                bl.RemoveTest(TestItem.TestID);
            }
            if (errorMessages.Any())
            {
                string err = ":יש לתקן את השגיאות";
                foreach (var item in errorMessages)
                    err += "\n" + item;
                MessageBox.Show(err);
                return;
            }
            try
            {
                bl.UpdateTester(tester);
                this.Closing -= Window_Closing;
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Validation Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errorMessages.Add(e.Error.Exception.Message);
            else errorMessages.Remove(e.Error.Exception.Message);
        }
        #region WorkHoursConfiguration

        TimeSpan StartTime = new TimeSpan();
        TimeSpan EndTime = new TimeSpan();

        public int HourStart
        {
            get { return StartTime.Hours; }
            set
            {
                if (value < 0 || value > 24)
                    throw new Exception("השעה לא תקינה");
                StartTime = new TimeSpan((int)Day, value, MinuteStart, 00);
            }
        }

        public int MinuteStart
        {
            get { return StartTime.Minutes; }
            set
            {
                if (value < 0 || value > 60)
                    throw new Exception("השעה לא תקינה");
                StartTime = new TimeSpan((int)Day, HourStart, value, 00);
            }
        }

        public int HourEnd
        {
            get { return EndTime.Hours; }
            set
            {
                if (value < 0 || value > 24)
                    throw new Exception("השעה לא תקינה");
                EndTime = new TimeSpan((int)Day, value, MinuteEnd, 00);
            }
        }

        public int MinuteEnd
        {
            get { return EndTime.Minutes; }
            set
            {
                if (value < 0 || value > 60)
                    throw new Exception("השעה לא תקינה");
                EndTime = new TimeSpan((int)Day, HourEnd, value, 00);
            }
        }

        public BE.WeekDays Day
        {
            get { return (BE.WeekDays)StartTime.Days; }
            set
            {
                StartTime = new TimeSpan((int)value, HourStart, MinuteStart, 00);
                EndTime = new TimeSpan((int)value, HourEnd, MinuteEnd, 00);
            }
        }

        public List<Test> TestsToRemove11 { get => TestsToRemove1; set => TestsToRemove1 = value; }

        /// <summary>
        /// Add TimePeriod Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTimePeriodButton_Click(object sender, RoutedEventArgs e)
        {
            if (timeErrorMessages.Any())
            {
                string err = "Exception:";
                foreach (var item in timeErrorMessages)
                    err += "\n" + item;
                MessageBox.Show(err);
                return;
            }
            try
            {
                Day = (BE.WeekDays)this.DayComboBox.SelectedItem;
            }
            catch (Exception)
            {
                MessageBox.Show("בחר יום", "שגיאה - לא נבחר יום", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                tester.WorkHours.Add(new BE.TimePeriod(StartTime, EndTime));
                tester.WorkHours = tester.WorkHours;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה בהזנת זמן העבודה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.WorkHoursDataGrid.ItemsSource = null;
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
        }

        #endregion

        /// <summary>
        /// WorkHours DataGrid Auto Generating Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkHoursDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "OnetimePeriod")
                e.Column.Header = "זמני עבודה";
        }

        /// <summary>
        /// WorkHours DataGrid Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkHoursDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (dynamic item in e.AddedItems)
            {
                timePeriodsToRemove.Add(tester.WorkHours.Where(TP => TP.ToString() == item.OnetimePeriod).First());
            }
            try
            {
            foreach (dynamic item in e.RemovedItems)
                {
                    timePeriodsToRemove.Remove(tester.WorkHours.Where(TP => TP.ToString() == item.OnetimePeriod).First());
                }
            }
            catch (Exception)
            {}
        }

        /// <summary>
        /// RemoveTimePeriod Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTimePeriod_Click(object sender, RoutedEventArgs e)
        {
            string RemovingTimePeriods = "";
            TestsToRemove2 = bl.GetAllTests(test => test.Time > DateTime.Now && timePeriodsToRemove.Any(t => tester.ID == test.TesterID
                && t.Start.Days == (int)test.Time.DayOfWeek
                && t.Start.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) <= test.Time.TimeOfDay
                && t.End.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) >= test.Time.TimeOfDay + BE.Configuration.TestTimeSpan)).ToList();
            //if (timePeriodsToRemove.Any(t=> bl.GetAllTests(test=> tester.ID == test.TesterID
            //    && t.Start.Days == (int)test.Time.DayOfWeek 
            //    && t.Start.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) <= test.Time.TimeOfDay 
            //    && t.End.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) >= test.Time.TimeOfDay + BE.Configuration.TestTimeSpan).Any()))
            if (TestsToRemove2.Any())
            {
                RemovingTimePeriods += " בלחיצה על \"שמור\" ימחקו גם הטסטים שמתוכננים " + (TestsToRemove2.Count() > 1 ? "לזמנים אלו: \n" : "לזמן זה: \n");
                foreach (var TestItem in TestsToRemove2)
                {
                    RemovingTimePeriods += TestItem.Time.ToString("dd/MM/yyyy HH:mm") + ", ";
                }
                RemovingTimePeriods.TrimEnd(',', ' ');
                RemovingTimePeriods += "\n\n";
                string messegeBody = "?אתה בטוח שאתה רוצה למחוק את " + timePeriodsToRemove.Count + (timePeriodsToRemove.Count == 1 ? " הזמן שנבחר\n\n" : " הזמנים שנבחרו\n\n")
                    + RemovingTimePeriods + " ניתן לשנות זמנים. עדכון יתבצע בלחיצה על \"שמור\". ";
                MessageBoxResult result = MessageBox.Show(messegeBody, "אישור מחיקה", MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                {
                    TestsToRemove2 = null;
                    return;
                }
                TestsToRemove1.AddRange(TestsToRemove2);
                //MessageBox.Show("לא ניתן להסיר זמני עבודה שכבר שמורים לטסטים", "שגיאה - זמן העבודה שמור לטסט", MessageBoxButton.OK, MessageBoxImage.Error);
                //return;
            }
            //removes WorkHours
            foreach (var item in timePeriodsToRemove)
            {
                tester.WorkHours.Remove(item);
            }
            this.WorkHoursDataGrid.ItemsSource = null;
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
        }

        /// <summary>
        /// Time validation Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added && e.Error.Exception != null)
                timeErrorMessages.Add(e.Error.Exception.Message);
            else timeErrorMessages.Remove(e.Error.Exception.Message);
        }

        /// <summary>
        /// WindowClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.tester.ToString() != bl.GetAllTesters(t=> t.ID == tester.ID).FirstOrDefault().ToString())
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// AddressPicker Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressPicker_TextChanged(object sender, EventArgs e)
        {
            tester.Address = this.addressPicker.Address;
        }

        /// <summary>
        /// Time Got Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_GotFocus(object sender, RoutedEventArgs e)
        {
            var t = e.OriginalSource as TextBox;
            if (t != null)
                t.SelectAll();
        }
    }
}
