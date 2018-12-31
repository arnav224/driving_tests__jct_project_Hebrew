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
        string body;
        string title = "מועד הטסט שלך מתקרב";
        string messege;
        string URL;
        string buttonText = "נווט למיקום הטסט";
        BE.Test test;
        BE.Trainee trainee;
        public SendingEmail(BE.Test _test)
        {
            InitializeComponent();
            this.test = _test;
            trainee = bl.GetAllTrainees(t => t.ID == test.TraineeID).First();
            if (test.RemeinderEmailSent != null)
                this.lable.Text = "לתלמיד זה כבר נשלחה תזכורת בתאריך "
                    + ((DateTime)test.RemeinderEmailSent).ToString("dd/MM/yyyy")
                        + " בשעה " + ((DateTime)test.RemeinderEmailSent).ToString("HH:mm")
                        +".\n  האם אתה בטוח שברצונך לשלוח שוב?";

            messege = trainee.FirstName + (trainee.Gender == BE.Gender.זכר ? " היקר " : " היקרה ") + @" רק רצינו להזכיר לך שמועד הטסט שלך מתקרב\n

    הטסט שלך יתקיים בתאריך " + test.Time.ToString("dd/MM/yyyy") + " בשעה " + test.Time.ToString("mm:HH") + ".\n" +
    "המיקום שנקבע לטסט הוא " + test.Address
    + "\nבהצלחה!";


            URL = "ttps://www.google.co.il/maps/place/" + test.Address.Replace(' ', '+');


            body = htmlFile
                .Replace("@@Title@@", title)
                .Replace("@@Text@@", messege)
                .Replace("@@LINK@@", URL)
                .Replace("@@BUTTON_TEXT@@", buttonText);
            this.showEmailWebBrowser.NavigateToString(body);

            var dp = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(
            TextBox.TextProperty,
            typeof(TextBox));
            dp.AddValueChanged(TextBox, (sender, args) =>
            {
                body = htmlFile
                    .Replace("@@Title@@", title)
                    .Replace("@@Text@@", messege + '\n' + TextBox.Text)
                    .Replace("@@LINK@@", URL)
                    .Replace("@@BUTTON_TEXT@@", buttonText);
                this.showEmailWebBrowser.NavigateToString(body);
            });
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //loading gif
            dynamic doc = showEmailWebBrowser.Document;
            var htmlText = doc.documentElement.InnerHtml;
            if (BE.Tools.SendingEmail(trainee.MailAddress, title, htmlText))
            {
                MessageBox.Show("המייל נשלח בהצלחה", "", MessageBoxButton.OK,
                               MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                bl.UpdateEmailSendingTime(test.TestID, null, DateTime.Now);
            }
            else
            {
                 MessageBox.Show("שגיאה בשליחת המייל", "", MessageBoxButton.OK,
                          MessageBoxImage.Error, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            }
            this.Close();
        }
    }
}
