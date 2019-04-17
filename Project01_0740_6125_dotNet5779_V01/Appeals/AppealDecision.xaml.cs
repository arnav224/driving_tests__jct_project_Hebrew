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
    /// Interaction logic for AppealDecision.xaml
    /// </summary>
    public partial class AppealDecision
    {
        BL.IBL bl = BL.Factory.GetInstance();
        BE.Test test;

        /// <summary>
        /// Appeal Decision ctor
        /// </summary>
        /// <param name="Oldtest"></param>
        public AppealDecision(BE.Test Oldtest)
        {
            test = Oldtest;
            InitializeComponent();
            this.DataContext = test;
            this.AppealTextBox.SelectionChanged += AppealTextBox_SelectionChanged;
            this.WelcomeTextBlock.Text = "פרטי הטסט:\n" + test +'\n'
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
            + ", שמירה על הימין - " + test.Indices["שמירה על הימין"] + ".\n"
            + "טענות התלמיד:\n" + test.AppealTest.TraineeNotes + '\n';
        }

        /// <summary>
        /// AppealTextBox Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppealTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.AppealTextBox.Text != "" && this.AppealTextBox.Text != null)
            {
                this.SendButton.IsEnabled = true;
                this.SendButton.ToolTip = null;
            }
        }

        /// <summary>
        /// Button Click to save the decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test.AppealTest.appealStatus = (this.DecisionCheckBox.IsChecked == true ? BE.AppealStatus.התקבל : BE.AppealStatus.נדחה);
            test.Passed = this.DecisionCheckBox.IsChecked;
            test.AppealTest.DecisionTime = DateTime.Now;
            test.AppealTest.Decision = this.AppealTextBox.Text;
            try
            {
                bl.TestAppeal(test);
                this.DialogResult = true;
                bl.SendEmail(test.TestID, BE.EmailType.Appeal);
                this.Closing -= Window_Closing;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.AppealTextBox.Text != "" || this.AppealTextBox.Text != null)
            {
                MessageBoxResult result = MessageBox.Show("?לצאת בלי לשמור שינויים", "", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }

        }

    }
}
