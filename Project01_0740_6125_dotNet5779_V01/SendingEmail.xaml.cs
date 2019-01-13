using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SendingEmail.xaml
    /// </summary>
    public partial class SendingEmail 
    {
        BL.IBL bl = BL.Factory.GetInstance();
        string htmlFile = File.ReadAllText("emails/TestRemeinder.html");
        BE.Trainee trainee;
        BE.Test Test;

        /// <summary>
        /// SendingEmail ctor
        /// </summary>
        /// <param name="test"></param>
        public SendingEmail(BE.Test test)
        {
            InitializeComponent();
            trainee = bl.GetAllTrainees(t => t.ID == test.TraineeID).First();
            Test = test;
            if (test.RemeinderEmailSent != null)
                this.lable.Text = "לתלמיד זה כבר נשלחה תזכורת בתאריך "
                    + ((DateTime)test.RemeinderEmailSent).ToString("dd/MM/yyyy")
                        + " בשעה " + ((DateTime)test.RemeinderEmailSent).ToString("HH:mm")
                        + ".\n  האם אתה בטוח שברצונך לשלוח שוב?";

            this.showEmailWebBrowser.NavigateToString(bl.GetEmailTemltateTestRemeinder(test.TestID));
            var dp = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(
            TextBox.TextProperty,
            typeof(TextBox));
            dp.AddValueChanged(TextBox, (sender, args) =>
            {
                this.showEmailWebBrowser.NavigateToString(bl.GetEmailTemltateTestRemeinder(test.TestID, TextBox.Text));
            });
        }

        /// <summary>
        /// SendButton Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic doc = showEmailWebBrowser.Document;
            ((MainWindow)Application.Current.MainWindow).htmlText = doc.documentElement.InnerHtml;



            //dynamic doc = showEmailWebBrowser.Document;
            //var htmlText = doc.documentElement.InnerHtml;
            //if (BE.Tools.SendingEmail(trainee.MailAddress, "מועד הטסט שלך מתקרב", htmlText))
            //{
            //    MessageBox.Show("המייל נשלח בהצלחה", "", MessageBoxButton.OK,
            //                   MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            //    bl.UpdateEmailSendingTime(Test.TestID, null, DateTime.Now);
            //}
            //else
            //{
            //     MessageBox.Show("שגיאה בשליחת המייל", "", MessageBoxButton.OK,
            //              MessageBoxImage.Error, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            //}
            DialogResult = true;
            this.Close();
        }
    }
}
