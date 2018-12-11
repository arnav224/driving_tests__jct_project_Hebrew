using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;

namespace BE
{
    public static class Tools
    {

        public static List<string> Maps_GetPlaceAutoComplete(string str)
        {
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

        //https://github.com/maximn/google-maps/blob/master/README.md
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

        public static bool SendingEmail(string recipients, string subject, string body)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(Configuration.SenderEmailAddress.Address, Configuration.EmailServerPasword),
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




    }
}
