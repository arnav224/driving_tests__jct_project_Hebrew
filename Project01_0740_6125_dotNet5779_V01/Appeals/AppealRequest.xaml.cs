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
    /// Interaction logic for AppealRequest.xaml
    /// </summary>
    public partial class AppealRequest : Window
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test = ((MainWindow)Application.Current.MainWindow).selectedTests[0];
        string OldTestTraineeNotes;
        public AppealRequest()
        {
            InitializeComponent();
            this.DataContext = test;
            test.AppealTest = new BE.AppealTest();
            OldTestTraineeNotes = test.AppealTest.TraineeNotes = "";
            this.AppealTextBox.SelectionChanged += AppealTextBox_SelectionChanged;
            BE.Trainee trainee = bl.GetAllTrainees(t=> t.ID == test.TraineeID).First();
            this.WelcomeTextBlock.Text = "שלום לך " + trainee.FirstName + ' ' + trainee.LastName + "! " 
                + "להלן פרטי הטסט. תוכל לכתוב כאן את טענותיך כדי לערער על התוצאות.\n" + test + '\n' 
             + "בטיחות - " + test.Indices["בטיחות"]
            + ", שליטה בהגה - " + test.Indices["שליטה בהגה"]
            + ", שליטה בהילוכים - " + test.Indices["שליטה בהילוכים"]
            + ", הסתכלות במראות - " + test.Indices["הסתכלות במראות"]
            + ", מתן זכות קדימה - " + test.Indices["מתן זכות קדימה"]
            + ", מהירות - " + test.Indices["מהירות"]
            + ", איתות - " + test.Indices["איתות"]
            + ", האצה והאטה בבטחה - " + test.Indices["האצה והאטה בבטחה"]
            + ", ציות לתמרורים - " + test.Indices["ציות לתמרורים"] + '\n'
            + ", שמירה רווח - " + test.Indices["שמירה רווח"]
            + ", בטיחות הולכי רגל - " + test.Indices["בטיחות הולכי רגל"]
            + ", עקיפות - " + test.Indices["עקיפות"]
            + ", חנייה - " + test.Indices["חנייה"]
            + ", פניות - " + test.Indices["פניות"]
            + ", השתלבות בתנועה - " + test.Indices["השתלבות בתנועה"]
            + ", אביזרי בטיחות - " + test.Indices["אביזרי בטיחות"]
            + ", נסיעה לאחור - " + test.Indices["נסיעה לאחור"]
            + ", שמירה על הימין - " + test.Indices["שמירה על הימין"] + '.';
        }

        private void AppealTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.AppealTextBox.Text != OldTestTraineeNotes)
                this.SendButton.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test.AppealTest.appealStatus = BE.AppealStatus.ממתין;
            test.AppealTest.RequestTime = DateTime.Now;
            test.AppealTest.TraineeNotes = this.AppealTextBox.Text;
            try
            {
                bl.UpdateTestResult(test);
                this.Closing -= Window_Closing;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.AppealTextBox.Text != OldTestTraineeNotes)
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }

        }
    }
}
