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
    /// Interaction logic for UpdateTest.xaml
    /// </summary>
    public partial class UpdateTest : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test = ((MainWindow)Application.Current.MainWindow).selectedTests[0];
        List<string> errorMessages = new List<string>();
        private int hout;
        public int Hour
        {
            get { return hout; }
            set
            {
                if (value < 0 || value > 24)
                    throw new Exception("השעה לא תקינה");
                hout = value;
            }
        }
        private int minute;
        public int Minute
        {
            get { return minute; }
            set
            {
                if (value < 0 || value > 60)
                    throw new Exception("השעה לא תקינה");
                minute = value;
            }
        }

        public UpdateTest()
        {
            InitializeComponent();
            Hour = test.Time.Hour;
            Minute = test.Time.Minute;
            this.DataContext = test;
            this.Time_hour.DataContext = this;
            this.Time_minutes.DataContext = this;
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
                test.Time = test.Time.AddMinutes(-test.Time.Minute);
                test.Time = test.Time.AddMinutes(Minute);
                test.Time = test.Time.AddHours(-test.Time.Hour);
                test.Time = test.Time.AddHours(Hour);
                bl.UpdateTest(test);
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
            if (e.Action == ValidationErrorEventAction.Added)
                errorMessages.Add(e.Error.Exception.Message);
            else errorMessages.Remove(e.Error.Exception.Message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.test.ToString() != bl.GetAllTests(t => t.TestID == test.TestID).FirstOrDefault().ToString())
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }
    }
}
