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
    /// Interaction logic for AddTrainee.xaml
    /// </summary>
    public partial class AddTrainee : Window
    {
        BL.IBL bl;
        BE.Trainee trainee;
        List<string> errorMessages = new List<string>();
        public AddTrainee()
        {
            InitializeComponent();
            bl = BL.Factory.GetInstance();
            this.gearBoxTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearBoxType));
            this.genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.vehicleComboBox.ItemsSource = Enum.GetValues(typeof(BE.Vehicle));
            trainee = new BE.Trainee();
            this.DataContext = trainee;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
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
                bl.AddTrainee(trainee);
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
            if (this.trainee.ToString() != new BE.Trainee().ToString())
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                              MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void AddressPicker_TextChanged(object sender, EventArgs e)
        {
            trainee.Address = this.addressPicker.Address;
        }
    }
}
