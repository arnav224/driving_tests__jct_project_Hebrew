using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BE
{
    public static class Configuration
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

        // Program Setting Variables:
        static public int MinimumTesterAge = 40;
        static public int MaximumTestsInWeek = 40;
        static public int MaximumTesterAge = 70;
        static public int MinimumTaineeAge = 18;
        static public int MinimumDaysBetweenTests = 7;
        static public int MinimumDrivingLessons = 20;
        static public int PassingGrade = 70;
        static public TimeSpan TestTimeSpan = new TimeSpan(0, 30, 0);
        static public int WorkStartHour = 7;
        static public int WorkEndHour = 19;
        static public int FridayWorkEndHour = 14;


        //Email Configuration:
        public static readonly MailAddress SenderEmailAddress = new MailAddress("avraham224@gmail.com", "Y&A בית ספר לנהיגה");
        public static readonly string EmailServerPasword = "hluuarmauvxfpzfs";
        //internal static string GoogleMapsApiKey = "AIzaSyA2GzC_8tGKa16ta2bQG-H1U0vjwQYOWns";
        public static string GoogleMapsApiKey = "AIzaSyBzz5Hd1UJW7NKf27JvD4HB0nBfLM4qfJQ";
        internal static string GoogleDistanceURL = "https://maps.googleapis.com/maps/api/distancematrix/";

        /// <summary>
        /// deep cloning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
                return default(T);
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "source");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
