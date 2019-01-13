using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Net;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Windows.Controls;

namespace BE
{
    public static class Tools
    {
        /// <summary>
        /// AutoComplete to Address
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> Maps_GetPlaceAutoComplete(string str)
        {
            if (str == null || str.Length == 0)
               return new List<string>() { "" };

            List<string> result = new List<string>();
            PlaceAutocompleteRequest request = new PlaceAutocompleteRequest();
            request.ApiKey = "AIzaSyBzz5Hd1UJW7NKf27JvD4HB0nBfLM4qfJQ";
            request.Input = str;
            request.Language = "he";
            var response = GoogleMaps.PlaceAutocomplete.Query(request);

            foreach (var item in response.Results)
            {
                result.Add(item.Description);
            }
            return result;
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        /// <summary>
        /// Distance road trip between two addresses
        /// https://github.com/maximn/google-maps/blob/master/README.md
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destinationAddress"></param>
        /// <returns></returns>
        public static int Maps_DrivingDistance(string sourceAddress, string destinationAddress)
        {
            //@@ Currently disabled manually
            //return new Random().Next(2000,3000);
            //@@


            Leg leg = null;
            try
            {
                var drivingOirectionRequest = new DirectionsRequest
                {
                    TravelMode = TravelMode.Driving,
                    Origin = sourceAddress,
                    Destination = destinationAddress,
                    ApiKey = Configuration.GoogleMapsApiKey,
                };
                DirectionsResponse drivingDirection = GoogleMaps.Directions.Query(drivingOirectionRequest);
                Route route = drivingDirection.Routes.First();
                leg = route.Legs.First();
                return leg.Distance.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendingEmail(string recipients, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(BE.Configuration.SMTP_Server, BE.Configuration.SMTP_Port)
                {
                    Credentials = new NetworkCredential(Configuration.SenderEmailAddress, Configuration.EmailServerPasword),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage(recipients, recipients) { Body = body, IsBodyHtml = true, Subject = subject, };
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static List<string> GetAddressSuggestionsGoogle(string input, string token)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/xml?input=" + input + "&key="
                      + Configuration.GoogleMapsApiKey + "&sessiontoken=" + token + "&components=country:il" + "&language=iw";
            try
            {
                var xml =  DownloadDataIntoXml(url);
                return (from adr in xml.Elements()
                        where adr.Name == "prediction"
                        select (string)adr.Element("description").Value
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static XElement DownloadDataIntoXml(string url)
        {
            //check the url
            if (url.ToLower().IndexOf("https:") > -1 || url.ToLower().IndexOf("http:") > -1)
            {
                //download the data into an xml
                var wc = new WebClient();
                var response = wc.DownloadData(url);
                var content = Encoding.UTF8.GetString(response);
                var xml = XElement.Parse(content);

                //check the request state
                if (xml.Element("status").Value != "OK")
                    throw new Exception();
                    //throw new GoogleAddressException("Google returns the next error: ", xml.Element("status").Value);

                return xml;
            }

            //throw new GoogleAddressException("Google URL is not correct", "WRONG_URL");
            throw new Exception();
        }

        static public void TestReminderSendEmail(Test test, Trainee trainee)
        {
           new Thread(() =>
            {
                try
                {
                    string MessageTosend = File.ReadAllText("emails/TestRemeinder/1.txt")
                        + "מועד הטסט שלך מתקרב" + File.ReadAllText("emails/TestRemeinder/2.txt")
                        + trainee.FirstName + (trainee.Gender == BE.Gender.זכר ? " היקר " : " היקרה ") + @" רק רצינו להזכיר לך שמועד הטסט שלך מתקרב\n

    הטסט שלך יתקיים בתאריך " + test.Time.ToString("dd/MM/yyyy") + " בשעה " + test.Time.ToString("HH:mm") + ".\n" +
    "המיקום שנקבע לטסט הוא " + test.Address 
    + "\nבהצלחה!"
                        + File.ReadAllText("emails/TestRemeinder/3.txt")
                        + "ttps://www.google.co.il/maps/place/" + test.Address.Replace(' ', '+')
                        + File.ReadAllText("emails/TestRemeinder/4.txt")
                        + "נווט למיקום הטסט"
                        + File.ReadAllText("emails/TestRemeinder/5.txt");
                    if (SendingEmail(trainee.MailAddress, "מועד הטסט שלך מתקרב", MessageTosend))
                        test.RemeinderEmailSent = DateTime.Now;

                }
                catch (Exception)
                {
                }

            }).Start();
        }

        static public void TestCancelationSendEmail(Test test, Trainee trainee)
        {
            new Thread(() =>
            {
                try
                {
                    string MessageTosend = File.ReadAllText("emails/TestCancelation/1.txt")
                        + "הטסט שלך התבטל" + File.ReadAllText("emails/TestCancelation/2.txt")
                        + trainee.FirstName + (trainee.Gender == BE.Gender.זכר ? " היקר " : " היקרה ")
                        + "לצערנו הטסט שלך שנקבע לתאריך " + test.Time.ToString("dd/MM/yyyy")
                        + " בשעה " + test.Time.ToString("HH:mm")
                        + " התבטל.\n"
                        + "תוכל לקבוע מועד חדש לטסט.\n"
                        + "בהצלחה!"
                        + File.ReadAllText("emails/TestCancelation/3.txt");
                    if (SendingEmail(trainee.MailAddress, "הטסט שלך התבטל", MessageTosend))
                        test.RemeinderEmailSent = DateTime.Now;

                }
                catch (Exception)
                {
                }

            }).Start();
        }


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
        public static bool ValidateID(string IDNum)
        {
            // Validate correct input
            if (!System.Text.RegularExpressions.Regex.IsMatch(IDNum, @"^\d{5,9}$"))
                return false;

            // The number is too short - add leading 0000
            while (IDNum.Length < 9)
                IDNum = '0' + IDNum;

            // CHECK THE ID NUMBER
            int mone = 0;
            int incNum;
            for (int i = 0; i < 9; i++)
            {
                incNum = Convert.ToInt32(IDNum[i].ToString()) * ((i % 2) + 1);
                if (incNum > 9)
                    incNum -= 9;
                mone += incNum;
            }
            return (mone % 10 == 0);
        }

        //todo זמני
        public static string ActiveValidateID(string IDNum)
        {
            // Validate correct input
            //if (!System.Text.RegularExpressions.Regex.IsMatch(IDNum, @"^\d{5,9}$"))
            //    return false;

            // The number is too short - add leading 0000
            while (IDNum.Length < 9)
                IDNum = '0' + IDNum;

            // CHECK THE ID NUMBER
            int mone = 0;
            int incNum;
            for (int i = 0; i < 9; i++)
            {
                incNum = Convert.ToInt32(IDNum[i].ToString()) * ((i % 2) + 1);
                if (incNum > 9)
                    incNum -= 9;
                mone += incNum;
            }
            string result = "";
            for (int i = 0; i < 8; i++)
                result += IDNum[i];
            return result + ((10 - (mone % 10) + Convert.ToInt32(IDNum[8].ToString())) % 10 ).ToString();
        }

    }
}
