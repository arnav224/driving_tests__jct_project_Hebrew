using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using BE.Properties;

namespace BE
{
    public class Configuration
    {
        static public Random Random = new Random();
        private static int LastTestId = 10000000;
        /// <summary>
        /// Serial number for Tests
        /// </summary>
        /// <returns></returns>
        public static int NextTestID()
        {
            if (LastTestId == int.MaxValue)
                throw new OutOfMemoryException();
            return LastTestId++;
        }



        static public int MinimumTaineeAge
        {
            get { return (int)Settings.Default["MinimumTaineeAge"]; }
            set
            {
                if (value < 16 || value > 50)
                    throw new Exception();
                Settings.Default["MinimumTaineeAge"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int MinimumTesterAge
        {
            get { return (int)Settings.Default["MinimumTesterAge"]; }
            set
            {
                if (value < 18 || value > 60)
                    throw new Exception();
                Settings.Default["MinimumTesterAge"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int MaximumTestsInWeek
        {
            get { return (int)Settings.Default["MaximumTestsInWeek"]; }
            set
            {
                if (value < 0 || value > 99)
                    throw new Exception();
                Settings.Default["MaximumTestsInWeek"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int MaximumTesterAge
        {
            get { return (int)Settings.Default["MaximumTesterAge"]; }
            set
            {
                if (value < 18 || value > 120)
                    throw new Exception();
                Settings.Default["MaximumTesterAge"] = value;
                Properties.Settings.Default.Save();
            }
        }



        static public int MinimumDaysBetweenTests
        {
            get { return (int)Settings.Default["MinimumDaysBetweenTests"]; }
            set
            {
                if (value < 0 || value > 999)
                    throw new Exception();
                Settings.Default["MinimumDaysBetweenTests"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int MinimumDrivingLessons
        {
            get { return (int)Settings.Default["MinimumDrivingLessons"]; }
            set
            {
                if (value < 0 || value > 999)
                    throw new Exception();
                Settings.Default["MinimumDrivingLessons"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int PassingGrade
        {
            get { return (int)Settings.Default["PassingGrade"]; }
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception();
                Settings.Default["PassingGrade"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public TimeSpan TestTimeSpan
        {
            get { return (TimeSpan)Settings.Default["TestTimeSpan"]; }
        }
        static public int TestTimeSpanInMinutes
        {
            get { return TestTimeSpan.Minutes; }
            set
            {
                if (value < 0 || value > 60)
                    throw new Exception();
                Settings.Default["TestTimeSpan"] = new TimeSpan(0, value, 0);
            }
        }

        static public int WorkStartHour
        {
            get { return (int)Settings.Default["WorkStartHour"]; }
            set
            {
                if (value < 0 || value > 20 || value > WorkEndHour)
                    throw new Exception();
                Settings.Default["WorkStartHour"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static private int allowToAddTest_DaysInAdvance = 90;
        static public int AllowToAddTest_DaysInAdvance
        {
            get { return allowToAddTest_DaysInAdvance; }
            set { allowToAddTest_DaysInAdvance = value; }
        }

        static public int WorkEndHour
        {
            get { return (int)Settings.Default["WorkEndHour"]; }
            set
            {
                if (value < 5 || value > 24 || value < WorkStartHour)
                    throw new Exception();
                Settings.Default["WorkEndHour"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int FridayWorkEndHour
        {
            get { return (int)Settings.Default["FridayWorkEndHour"]; }
            set
            {
                if (value < 0 || value > 16)
                    throw new Exception();
                Settings.Default["FridayWorkEndHour"] = value;
                Properties.Settings.Default.Save();
            }
        }



        //Email Configuration:
        static public string SenderEmailAddress
        {
            get { return (string)Settings.Default["SenderEmailAddress"]; }
            set
            {
                new MailAddress(value); //validation
                Settings.Default["SenderEmailAddress"] = value;
                Properties.Settings.Default.Save();
            }
        }

        //static private bool autoSendingEmails;
        static public bool AutoSendingEmails
        {
            get { return (bool)Settings.Default["AutoSendingEmails"]; }
            set
            {
                Settings.Default["AutoSendingEmails"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public bool AutoSendingEmailsAboutAddingAndCancalation
        {
            get { return (bool)Settings.Default["AutoSendingEmailsAboutAddingAndCancalation"]; }
            set
            {
                Settings.Default["AutoSendingEmailsAboutAddingAndCancalation"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int SendingEmails_DaysInAdvance
        {
            get { return (int)Settings.Default["SendingEmails_DaysInAdvance"]; }
            set
            {
                if (value < 0)
                    throw new Exception();
                Settings.Default["SendingEmails_DaysInAdvance"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public string SMTP_Server
        {
            get { return (string)Settings.Default["SMTP_Server"]; }
            set
            {
                Settings.Default["SMTP_Server"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public int SMTP_Port
        {
            get { return (int)Settings.Default["SMTP_Port"]; }
            set
            {
                if (value < 0)
                    throw new Exception();
                Settings.Default["SMTP_Port"] = value;
                Properties.Settings.Default.Save();
            }
        }

        static public string EmailServerPasword
        {
            get { return (string)Settings.Default["EmailServerPasword"]; }
            set
            {
                Settings.Default["EmailServerPasword"] = value;
                Properties.Settings.Default.Save();
            }
        }
        //static private string myVar = "fhmbqwxfzdjefqhd"; //"hluuarmauvxfpzfs"
        //static public string EmailServerPasword
        //{
        //    get { return myVar; }
        //    set { myVar = value; }
        //}


        static public string GoogleMapsApiKey
        {
            get { return (string)Settings.Default["GoogleMapsApiKey"]; }
            set
            {
                Settings.Default["GoogleMapsApiKey"] = value;
                Properties.Settings.Default.Save();
            }
        }

    }
}
