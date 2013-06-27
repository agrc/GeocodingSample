using System.Globalization;

namespace GeocodingSample
{
    public class ResultContainer<T> where T : class
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }

    public class GeocodeResult : Suggestable
    {
        public Location Location { get; set; }

        public double Score { get; set; }

        public string Locator { get; set; }

        public string MatchAddress { get; set; }

        public string InputAddress { get; set; }

        public string StandardizedAddress { get; set; }

        public string AddressGrid { get; set; }
    }

    public abstract class Suggestable
    {
        public virtual Candidate[] Candidates { get; set; }
    }

    public class Candidate
    {
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;

                if (string.IsNullOrEmpty(_address)) return;

                var parts = _address.Split(new[] {','});

                if (parts.Length != 3) return;

                AddressGrid = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[1].Trim().ToLowerInvariant());
                _address = string.Join(",", parts[0], parts[2]).Trim();
            }
        }

        public Location Location { get; set; }

        public double Score { get; set; }

        public string Locator { get; set; }

        public string AddressGrid { get; set; }
    }

    public class Location
    {
        public Location(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }
    }
}