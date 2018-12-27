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
    /// Interaction logic for AddTester.xaml
    /// </summary>
    public partial class AddTester : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Tester tester = new BE.Tester();
        List<string> errorMessages = new List<string>();
        List<string> timeErrorMessages = new List<string>();
        List<BE.TimePeriod> timePeriodsToRemove = new List<BE.TimePeriod>();


        public AddTester()
        {
            InitializeComponent();
            this.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            this.DataContext = tester;
            this.WorkHoursGrid.DataContext = this;
            this.DayComboBox.ItemsSource = Enum.GetValues(typeof(BE.WeekDays));
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorMessages.Any())
            {
                string err = "התגלו שגיאות בטופס:";
                foreach (var item in errorMessages)
                    err += "\n" + item;
                MessageBox.Show(err);
                return;
            }
            try
            {
                bl.AddTester(tester);
                this.Closing -= Window_Closing;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added && e.Error.Exception != null)
                errorMessages.Add(e.Error.Exception.Message);
            else errorMessages.Remove(e.Error.Exception.Message);
        }

        private void time_validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added && e.Error.Exception != null)
                timeErrorMessages.Add(e.Error.Exception.Message);
            else timeErrorMessages.Remove(e.Error.Exception.Message);
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

        private void RemoveTimePeriod_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in timePeriodsToRemove)
            {
                tester.WorkHours.Remove(item);
            }
            this.WorkHoursDataGrid.ItemsSource = null;
            this.WorkHoursDataGrid.ItemsSource = (from item in tester.WorkHours select new { OnetimePeriod = item.ToString() });
        }

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
            { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.tester.ToString() != new BE.Tester().ToString())
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                              MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void AddressPicker_TextChanged(object sender, EventArgs e)
        {
            tester.Address = this.addressPicker.Address;
        }


    }
}
