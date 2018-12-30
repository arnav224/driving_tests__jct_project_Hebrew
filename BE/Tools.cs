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

            //@@ Currently disabled manually
            //return new List<string>() {  str /*+ "מושבת"*/ };
            //@@

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
            return new Random().Next(2000,3000);
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
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(Configuration.SenderEmailAddress, Configuration.EmailServerPasword),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage(recipients, recipients) { Body = body };
                client.Send(mailMessage);
                //client.Send(Configuration.SenderEmailAddress, recipients, subject, body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static List<string> GetAddressSuggestionsGoogle(string input, string token)
        {
            //return Maps_GetPlaceAutoComplete(input);//@
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/xml?input=" + input +
                      "&types=address" /*+ "&location=31.728220,34.983749&radius=300000"*/ + "&key=" 
                      + Configuration.GoogleMapsApiKey + "&sessiontoken=" + token + /*"&components=country:il" +*/ "&language=iw";
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

        static public void sendTry1()
        {
    //        new Thread(() => 
    //        {
    //            try
    //            {
    //                string MessageTosend = File.ReadAllText("emails/TestRemeinder/1.txt")
    //                    + "מועד הטסט שלך מתקרב" + File.ReadAllText("emails/TestRemeinder/2.txt")
    //                    + @"חיים היקר, רק רצינו להזכיר לך שמועד הטסט שלך מתקרב
    //הטסט שלך יתקיים בתאריך XX בשעה YY  מומלץ להקדים.
    //בהצלחה! 

    //."
    //                    + File.ReadAllText("emails/TestRemeinder/3.txt")
    //                    + "ttps://www.google.co.il/maps/place/" + ""
    //                    + File.ReadAllText("emails/TestRemeinder/4.txt")
    //                    + "נווט למיקום הטסט"
    //                    + File.ReadAllText("emails/TestRemeinder/5.txt");

    //                var client = new SmtpClient("smtp.gmail.com", 587)
    //                {
    //                    Credentials = new NetworkCredential(Configuration.SenderEmailAddress.Address, Configuration.EmailServerPasword),
    //                    EnableSsl = true
    //                };
    //                MailMessage mailMessage = new MailMessage("avraham224@gmail.com", "avraham224@gmail.com") { Body = MessageTosend, IsBodyHtml = true, };
    //                client.Send(mailMessage);
    //                //client.Send(Configuration.SenderEmailAddress, recipients, subject, body);
    //                return true;
    //            }
    //            catch (Exception)
    //            {
    //                return false;
    //            }

            //}).Start();

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


    }
}
