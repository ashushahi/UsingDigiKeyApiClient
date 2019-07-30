using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiClient;
using ApiClient.Models;
using ApiClient.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UsingDigiKeyApiClient.Models;

namespace BomzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        
        // GET api/values
        [HttpGet("GetParts")]
        public async Task<string> Get([FromQuery]string partId, [FromQuery]string quantity, [FromQuery]string currency)
        {
           // string apiToken = "41c44903-fdc0-417c-abb9-6c79bbad03f1";
           //partId =string.IsNullOrEmpty(partId)? "GRM21BR71C225KA12L" : partId;
           // string mouserUrl = "https://api.mouser.com/api/v1/search/partnumber";
           // MouserRoot restResponse = new MouserRoot();
           // HttpClient client = new HttpClient();
           // Models.ValueRow pResponse = new Models.ValueRow();
           //// client.DefaultRequestHeaders.Add("Content-Type", "application/json");
           // try
           // {
           //     pResponse.PartNumber = partId;
           //     pResponse.Quantity =Convert.ToInt32( string.IsNullOrEmpty(quantity) ? "1" : quantity);
           //     Models.Rootobject bodyParam = new Models.Rootobject() { SearchByPartRequest = new Models.Searchbypartrequest() { mouserPartNumber = partId } };
           //     string json = JsonConvert.SerializeObject(bodyParam);
           //     mouserUrl += "?apiKey=";
           //     mouserUrl += apiToken;
           //     HttpRequestMessage mouserRequest = new HttpRequestMessage()
           //     {
           //         Method = HttpMethod.Post,
           //         RequestUri = new Uri(mouserUrl),
           //         Content = new StringContent(json, Encoding.UTF8, "application/json")

           //     };
                
           //     HttpResponseMessage response = await client.SendAsync(mouserRequest).ConfigureAwait(false);
           //     response.EnsureSuccessStatusCode();
           //     string responseBody = await response.Content.ReadAsStringAsync();
              
           //     restResponse = JsonConvert.DeserializeObject<MouserRoot>(responseBody);
           //     if (restResponse.SearchResults.Parts.Count() > 0)
           //     {

           //         pResponse.Manufacturer = restResponse.SearchResults.Parts[0].Manufacturer;
           //         pResponse.Mouser = new Models.Mouser() { LeadTime = restResponse.SearchResults.Parts[0].LeadTime, AvailableQuantity = restResponse.SearchResults.Parts[0].Availability ,PriceBreakups = new List<Models.PriceBreakUp>()};
           //         if (restResponse.SearchResults.Parts[0].PriceBreaks.Count() > 0)
           //         {
           //             for (int i = 0; i < restResponse.SearchResults.Parts[0].PriceBreaks.Count(); i++)
           //             {
           //                 pResponse.Mouser.PriceBreakups.Add(new Models.PriceBreakUp() { Price = restResponse.SearchResults.Parts[0].PriceBreaks[i].Price, Quantity = restResponse.SearchResults.Parts[0].PriceBreaks[i].Quantity.ToString() });
           //             }
           //         }
           //     }
           // }
           // catch (HttpRequestException e)
           // {
           //     throw;
           // }
            
           // return pResponse;



            var settings = ApiClientSettings.CreateFromConfigFile();
            string jsonFormatted = "";

            try
            {
                if (settings.ExpirationDateTime < DateTime.Now)
                {
                    // Let's refresh the token
                    var oAuth2Service = new OAuth2Service(settings);
                    var oAuth2AccessToken = oAuth2Service.RefreshTokenAsync().Result;
                    if (oAuth2AccessToken.IsError)
                    {
                        // Current Refresh token is invalid or expired 

                        return null;
                    }

                    settings.UpdateAndSave(oAuth2AccessToken);


                    Console.WriteLine("After call to refresh");
                    Console.WriteLine(settings.ToString());
                }

                var client = new ApiClientService(settings);
                var response = client.KeywordSearch("LM2904M").Result;

                // In order to pretty print the json object we need to do the following
                 jsonFormatted = JToken.Parse(response).ToString(Formatting.Indented);


            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return jsonFormatted;
        }

        // GET api/values/5
      
    }
}
