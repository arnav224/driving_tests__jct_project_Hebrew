using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;

namespace Tools
{
    public static class Maps
    {

        //static public int Distance(BE.Address address1, BE.Address address2)
        //{ // At this stage it does not calculate a distance but returns a random number.


        //    ////נסיון
        //    //foreach (var item in GetPlaceAutoComplete("ירושל").AsEnumerable())
        //    //{
        //    //    Console.WriteLine(item);
        //    //}
        //    return CalcDistance(address1.ToString(), address2.ToString());

        //    //return (865 * address1.HostNumber + 765 * address2.HostNumber) % 5000;


        //}



        //public static List<string> GetPlaceAutoComplete(string str)
        //{
        //    List<string> result = new List<string>();
        //    PlaceAutocompleteRequest request = new PlaceAutocompleteRequest();
        //    request.ApiKey = "AIzaSyBzz5Hd1UJW7NKf27JvD4HB0nBfLM4qfJQ";
        //    request.Input = str;
        //    request.Language = "he";
        //    var response = GoogleMaps.PlaceAutocomplete.Query(request);

        //    foreach (var item in response.Results)
        //    {
        //        result.Add(item.Description);
        //    }
        //    return result;
        //}


        ////https://github.com/maximn/google-maps/blob/master/README.md
        //public static int Distance(string source, string destination)
        //{
        //    Leg leg = null;
        //    try
        //    {
        //        var drivingOirectionRequest = new DirectionsRequest
        //        {
        //            TravelMode = TravelMode.Driving,
        //            Origin = source,
        //            Destination = destination,
        //            ApiKey = Configuration.GoogleMapsApiKey,
        //        };
        //        DirectionsResponse drivingDirection = GoogleMaps.Directions.Query(drivingOirectionRequest);
        //        Route route = drivingDirection.Routes.First();
        //        leg = route.Legs.First();
        //        return leg.Distance.Value;
        //    }
        //    catch (Exception)
        //    {
        //    return -1;
        //    }
            
        //}

    }
}
