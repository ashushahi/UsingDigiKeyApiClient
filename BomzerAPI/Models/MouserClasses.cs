using Newtonsoft.Json;

namespace UsingDigiKeyApiClient.Models
{

    public class MouserRoot
    {
        public SearchResults SearchResults
        {
            get; set;
        }
    }

    public class SearchResults
    {
        public PartMouser[] Parts
        {
            get; set;
        }
    }

    [JsonObject("Part")]
    public class PartMouser
    {
        public string Availability
        {
            get; set;
        }
        public string LeadTime
        {
            get; set;
        }
        public string Manufacturer
        {
            get; set;
        }
        public PriceBreak[] PriceBreaks
        {
            get; set;
        }
    }

    public class PriceBreak
    {
        public int Quantity
        {
            get; set;
        }
        public string Price
        {
            get; set;
        }
        public string Currency
        {
            get; set;
        }
    }
}
