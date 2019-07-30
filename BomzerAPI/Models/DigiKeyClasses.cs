using Newtonsoft.Json;

namespace UsingDigiKeyApiClient.Models
{

    public class DigiKeyRoot
    {
        public PartDigikey[] Parts
        {
            get; set;
        }
    }

    [JsonObject("Part")]
    public class PartDigikey
    {
        public Standardpricing[] StandardPricing
        {
            get; set;
        }
        public int QuantityOnHand
        {
            get; set;
        }
        public float UnitPrice
        {
            get; set;
        }
        public Manufacturername ManufacturerName
        {
            get; set;
        }
        public string ManufacturerLeadWeeks
        {
            get; set;
        }
        
    }

    public class Manufacturername
    {
        public string Id
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
    }

    public class Standardpricing
    {
        public int BreakQuantity
        {
            get; set;
        }
        public float UnitPrice
        {
            get; set;
        }
        public float TotalPrice
        {
            get; set;
        }
    }

}
