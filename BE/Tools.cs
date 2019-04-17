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

namespace BE
{
    public static class Tools
    {

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        /// <summary>
        /// chack if Internet Available
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// AutoComplete to Address
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
                WebClient wc = new WebClient();
                byte[] response = wc.DownloadData(url);
                string content = Encoding.UTF8.GetString(response);
                XElement xml = XElement.Parse(content);

                //check the request state
                if (xml.Element("status").Value != "OK")
                    throw new Exception();
                return xml;
            }
            throw new Exception();
        }


        /// <summary>
        /// deep cloning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {
            if (source == null)
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

        /// <summary>
        /// Validate ID
        /// </summary>
        /// <param name="IDNum"></param>
        /// <returns></returns>
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

    }
}
