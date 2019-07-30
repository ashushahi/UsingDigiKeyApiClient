using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BomzerAPI.Models
{
   public class ValueRow
    {
        [JsonProperty("partNumber")]
        public string PartNumber { get; set; }

        [JsonProperty("quantity")]
        public int Quantity
        {
            get; set;
        }
        [JsonProperty("manufacturer")]
        public string Manufacturer
        {
            get; set;
        }
[JsonProperty("digikey")]
        public DigiKey DigiK
        {
            get;set;
        }
        [JsonProperty("mouser")]
        public Mouser Mouser
        {
            get; set;
        }
   
    }
    public class DigiKey
    {
        [JsonProperty("price")]
        public string Price
        {
            get; set;
        }

        [JsonProperty("availableQuantity")]
        public string AvailableQuantity
        {
            get; set;
        }
        [JsonProperty("leadTime")]
        public string LeadTime
        {
            get; set;
        }

       [JsonProperty("priceBreakUp")]
        public List<PriceBreakUp> PriceBreakups
        {get;set;
        }
    }

    public class Mouser
    {
        [JsonProperty("price")]
        public string Price
        {
            get; set;
        }

        [JsonProperty("availableQuantity")]
        public string AvailableQuantity
        {
            get; set;
        }
        [JsonProperty("leadTime")]
        public string LeadTime
        {
            get; set;
        }

        [JsonProperty("priceBreakUp")]
        public List<PriceBreakUp> PriceBreakups
        {
            get; set;
        }
    }
    public class PriceBreakUp
    {
        public string Quantity
        {
            get;set;
        }
        public string Price
        {
            get; set;
        }
    }
}
