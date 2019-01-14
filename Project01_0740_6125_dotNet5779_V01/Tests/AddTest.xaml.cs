using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddTest.xaml
    /// </summary>
    public partial class AddTest 
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test = new BE.Test() {Time = DateTime.Now };
        //SortedSet<DateTime> avalibleDateTimes;
        List<string> errorMessages = new List<string>();

        /// <summary>
        /// Add Test ctor
        /// </summary>
        /// <param name="TraineeID"></param>
        public AddTest(string TraineeID = null)
        {
            InitializeComponent();
            this.DataContext = test;
            this.addressPicker.Address = test.Address;
            this.TraineeIDComboBox.ItemsSource = from item in bl.GetAllTrainees() select item.ID;
            if (TraineeID != null)
                this.TraineeIDComboBox.SelectedItem = TraineeID;
            this.DateTimePicker.DisplayDateStart = DateTime.Today;
            this.DateTimePicker.DisplayDateEnd = DateTime.Today.AddMonths(3);
        }
        /// <summary>
        /// Ad dButton Click to save the test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
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
                bl.AddTest(test);
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

        /// <summary>
        /// Window Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.test.Address != null || this.test.Time != new DateTime() || this.test.TraineeID != null)
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
            test.Address = this.addressPicker.Address;
        }

        /// <summary>
        /// TraineeIDComboBox Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            test.TraineeID = this.TraineeIDComboBox.SelectedItem.ToString();
        }

        /// <summary>
        /// AddressPicker Lost Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.addressPicker.Address != null && this.TraineeIDComboBox.SelectedItem != null)
            {
                //todo
                //BackgroundWorker worker = new BackgroundWorker();
                //worker.DoWork += (sender, e) => { Thread.Sleep(10000); e.Result = pair; };
                //worker.RunWorkerCompleted += (s, arg) =>
                //{
                //    RemoveNotification(pair);
                //};
                //worker.RunWorkerAsync(argument: messege);

                //this.dateDatePicker.
                //this.avalibleDateTimes = bl.avalibleDateTimes(test);
                //new Thread(() => { this.avalibleDateTimes = bl.avalibleDateTimes(test); }).Start();

                //this.DateTimePicker.IsEnabled = true;
                //this.DateTimePicker.ToolTip = null;
                //this.dateDatePicker.IsEnabled = true;
                //this.dateDatePicker.ToolTip = null;
            }
            else
            {
                //this.DateTimePicker.IsEnabled = false;
                //this.DateTimePicker.ToolTip = "יש לבחור תלמיד וכתובת תחילה";

                //todo
                //this.dateDatePicker.IsEnabled = false;
                //this.dateDatePicker.ToolTip = "יש לבחור תלמיד וכתובת תחילה";
            }
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
