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
        BL.IBL bl;
        BE.Tester tester;
        List<string> errorMessages = new List<string>();


        public AddTester()
        {
            InitializeComponent();
            bl = BL.Factory.GetInstance();
            this.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            tester = new BE.Tester();
            this.DataContext = tester;
            this.WorkHoursGrid.DataContext = this;
            this.DayComboBox.ItemsSource = Enum.GetValues(typeof(BE.WeekDays));
            this.AddTimePeriodButton.Click += AddTimePeriodButton_Click;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה בהזנת זמן העבודה", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.workHoursTextBlock.Text = this.printWorkHours();
        }

        string printWorkHours()
        {
            string result = "";
            foreach (var item in tester.WorkHours)
                result += item.ToString() + '\n';
            return result;
        }
        #endregion
    }
}
