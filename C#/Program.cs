using System;
using System.Collections.Generic;
using Nito.AsyncEx;

namespace GeocodingSample
{
    internal class Program
    {
        private static void Main()
        {
            AsyncContext.Run(() => MainAsync());
        }

        private static async void MainAsync()
        {
            // Create your api key at
            // https://developer.mapserv.utah.gov/secure/KeyManagement

            var g = new Geocoding("insert your api key here");
            var location = await g.Locate("123 South Main Street", "SLC", new Dictionary<string, object>
                {
                    {"score", 90},
                    {"spatialReference", 4326}
                });

            Console.WriteLine(location);
            Console.ReadKey();
        }
    }
}