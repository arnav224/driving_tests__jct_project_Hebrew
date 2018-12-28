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
        public static readonly MailAddress SenderEmailAddress = new MailAddress("avraham224@gmail.com", "Y&A בית ספר לנהיגה");
        public static readonly string EmailServerPasword = "hluuarmauvxfpzfs";
        //internal static string GoogleMapsApiKey = "AIzaSyA2GzC_8tGKa16ta2bQG-H1U0vjwQYOWns";
        public static string GoogleMapsApiKey = "AIzaSyBzz5Hd1UJW7NKf27JvD4HB0nBfLM4qfJQ";
        internal static string GoogleDistanceURL = "https://maps.googleapis.com/maps/api/distancematrix/";

    }
}
