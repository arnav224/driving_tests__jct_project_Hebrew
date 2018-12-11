using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using Newtonsoft.Json;
using System.Net.Mail;

using System.Runtime.Serialization;


//namespace Tools
//{
//    public static class Configuration
//    {
//        //Email Configuration:
//        internal static readonly MailAddress SenderEmailAddress = new MailAddress("avraham224@gmail.com");
//        internal static readonly string EmailServerPasword = "hluuarmauvxfpzfs";
//        //internal static string GoogleMapsApiKey = "AIzaSyA2GzC_8tGKa16ta2bQG-H1U0vjwQYOWns";
//        internal static string GoogleMapsApiKey = "AIzaSyBzz5Hd1UJW7NKf27JvD4HB0nBfLM4qfJQ";
//        internal static string GoogleDistanceURL = "https://maps.googleapis.com/maps/api/distancematrix/";


//        public static T Clone<T>(this T source)
//        {
//            if (Object.ReferenceEquals(source, null))
//            {
//                return default(T);
//            }
//            if (!typeof(T).IsSerializable)
//            {
//                throw new ArgumentException("The type must be serializable.", "source");
//            }
//            IFormatter formatter = new BinaryFormatter();
//            Stream stream = new MemoryStream();
//            using (stream)
//            {
//                formatter.Serialize(stream, source);
//                stream.Seek(0, SeekOrigin.Begin);
//                return (T)formatter.Deserialize(stream);
//            }

//            //var serialized = JsonConvert.SerializeObject(source);
//            //return JsonConvert.DeserializeObject<T>(serialized);
//        }
//    }

//}
