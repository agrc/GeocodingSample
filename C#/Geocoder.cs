using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Nustache.Core;

namespace GeocodingSample
{
    public class Geocoding
    {
        private const string UrlTemplate = "http://api.mapserv.utah.gov/api/v1/geocode/{{street}}/{{zone}}";

        public Geocoding(string apiKey)
        {
            ApiKey = apiKey;
        }

        private string ApiKey { get; set; }

        public async Task<Location> Locate(string street, string zone, Dictionary<string, object> options = null)
        {
            var url = Render.StringToString(UrlTemplate, new
                {
                    Street = street,
                    Zone = zone
                });

            if (options == null)
                options = new Dictionary<string, object>();

            options.Add("apiKey", ApiKey);
            url += "?" + string.Join("&", options.Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));

            var request = new HttpClient();

            var response = await request.GetAsync(url);

            var resultContainer = await response.Content.ReadAsAsync<ResultContainer<GeocodeResult>>();

            if (response.StatusCode != HttpStatusCode.OK || resultContainer.Status != (int) HttpStatusCode.OK)
            {
                Console.WriteLine("{0} {1} was not found. {2}", street, zone, resultContainer.Message);
                return null;
            }

            var result = resultContainer.Result;

            Console.WriteLine("match: {0} score [{1}]", result.Score, result.MatchAddress);

            return result.Location;
        }
    }
}