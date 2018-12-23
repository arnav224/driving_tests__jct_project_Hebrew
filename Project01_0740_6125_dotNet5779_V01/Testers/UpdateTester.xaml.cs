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
    /// Interaction logic for UpdateTester.xaml
    /// </summary>
    public partial class UpdateTester : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();
        List<string> errorMessages = new List<string>();
        BE.Tester tester;
        List<BE.TimePeriod> timePeriodsToRemove = new List<BE.TimePeriod>();
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
            this.AddTimePeriodButton.Click += AddTimePeriodButton_Click;
            List<string> worsHours = (from item in tester.WorkHours select item.ToString()).ToList();
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
                //List<string>( tester.WorkHours;// (IEnumerable<string>)from item in tester.WorkHours select item.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (errorMessages.Any())
            {
                string err = "Exception:";
                foreach (var item in errorMessages)
                    err += "\n" + item;
                MessageBox.Show(err);
                return;
            }
            try
            {
                bl.UpdateTester(tester);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void validation_Error(object sender, ValidationErrorEventArgs e)
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


        private void AddTimePeriodButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void WorkHoursDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "OnetimePeriod")
                e.Column.Header = "זמני עבודה";
        }

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
            catch (Exception)   //@
            {}
        }

        private void RemoveTimePeriod_Click(object sender, RoutedEventArgs e)
        {
            if (timePeriodsToRemove.Any(t=> bl.GetAllTests(test=> tester.ID == test.TesterID
                && t.Start.Days == (int)test.Time.DayOfWeek 
                && t.Start.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) <= test.Time.TimeOfDay 
                && t.End.Subtract(new TimeSpan(t.Start.Days, 0, 0, 0)) >= test.Time.TimeOfDay + BE.Configuration.TestTimeSpan).Any()))
            {
                MessageBox.Show("לא ניתן להסיר זמני עבודה שכבר שמורים לטסטים", "שגיאה - זמן העבודה שמור לטסט", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var item in timePeriodsToRemove)
            {
                tester.WorkHours.Remove(item);
            }
            this.WorkHoursDataGrid.ItemsSource = null;
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}
