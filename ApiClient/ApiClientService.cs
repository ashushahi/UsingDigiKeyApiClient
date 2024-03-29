//-----------------------------------------------------------------------
//
// THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTIES OF ANY KIND, EXPRESS, IMPLIED, STATUTORY, 
// OR OTHERWISE. EXPECT TO THE EXTENT PROHIBITED BY APPLICABLE LAW, DIGI-KEY DISCLAIMS ALL WARRANTIES, 
// INCLUDING, WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, 
// SATISFACTORY QUALITY, TITLE, NON-INFRINGEMENT, QUIET ENJOYMENT, 
// AND WARRANTIES ARISING OUT OF ANY COURSE OF DEALING OR USAGE OF TRADE. 
// 
// DIGI-KEY DOES NOT WARRANT THAT THE SOFTWARE WILL FUNCTION AS DESCRIBED, 
// WILL BE UNINTERRUPTED OR ERROR-FREE, OR FREE OF HARMFUL COMPONENTS.
// 
//-----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiClient.Exception;
using ApiClient.Models;
using ApiClient.OAuth2;


namespace ApiClient
{
    public class ApiClientService
    {
        private const string _CustomHeader = "Api-StaleTokenRetry";
      

        private ApiClientSettings _clientSettings;

        public ApiClientSettings ClientSettings
        {
            get { return _clientSettings; }
            set { _clientSettings = value; }
        }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the this instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ApiClientService(ApiClientSettings clientSettings)
        {
            if (clientSettings == null)
            {
                throw new ArgumentNullException(nameof(clientSettings));
            }
            ClientSettings = clientSettings;
            Initialize();
        }

        private void Initialize()
        {
            HttpClient = new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var authenticationHeaderValue = new AuthenticationHeaderValue("Authorization", ClientSettings.AccessToken);
            HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            HttpClient.DefaultRequestHeaders.Add("X-IBM-Client-ID", ClientSettings.ClientId);
            HttpClient.BaseAddress = new Uri("https://api.digikey.com");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void ResetExpiredAccessTokenIfNeeded()
        {
            if (_clientSettings.ExpirationDateTime < DateTime.Now)
            {
                // Let's refresh the token
                var oAuth2Service = new OAuth2Service(_clientSettings);
                var oAuth2AccessToken = oAuth2Service.RefreshTokenAsync().Result;
                if (oAuth2AccessToken.IsError)
                {
                    // Current Refresh token is invalid or expired 
                    Console.WriteLine("Current Refresh token is invalid or expired ");
                    return;
                }

                // Update the clientSettings
                _clientSettings.UpdateAndSave(oAuth2AccessToken);
                Console.WriteLine("ApiClientService::CheckifAccessTokenIsExpired() call to refresh");
                Console.WriteLine(_clientSettings.ToString());

                // Reset the Authorization header value with the new access token.
                var authenticationHeaderValue = new AuthenticationHeaderValue("Authorization", _clientSettings.AccessToken);
                HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
        }

        public async Task<string> KeywordSearch(string keyword)
        {
            var resourcePath = "/services/partsearch/v2/keywordsearch";

            var request = new KeywordSearchRequest
            {
                Keywords = keyword ?? "P5555-ND",
                RecordCount = 25
            };

            ResetExpiredAccessTokenIfNeeded();
            var postResponse = await PostAsJsonAsync(resourcePath, request);

            return GetServiceResponse(postResponse).Result;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string resourcePath, T postRequest)
        {
            
            try
            {
                var response = await HttpClient.PostAsJsonAsync(resourcePath, postRequest);
               

                //Unauthorized, then there is a chance token is stale
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (OAuth2Helpers.IsTokenStale(responseBody))
                    {
                    
                        await OAuth2Helpers.RefreshTokenAsync(_clientSettings);
                        

                        //Only retry the first time.
                        if (!response.RequestMessage.Headers.Contains(_CustomHeader))
                        {
                            HttpClient.DefaultRequestHeaders.Add(_CustomHeader, _CustomHeader);
                            HttpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Authorization", _clientSettings.AccessToken);
                            return await PostAsJsonAsync(resourcePath, postRequest);
                        }
                        else if (response.RequestMessage.Headers.Contains(_CustomHeader))
                        {
                            throw new ApiException($"Inside method {nameof(PostAsJsonAsync)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.StatusCode}", null);
                        }
                    }
                }

                return response;
            }
            catch (HttpRequestException hre)
            {
                
                throw;
            }
            catch (ApiException dae)
            {
                
                throw;
            }
        }

        protected async Task<string> GetServiceResponse(HttpResponseMessage response)
        {
            
            var postResponse = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    postResponse = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Response");
                Console.WriteLine("  Status Code : {0}", response.StatusCode);
                Console.WriteLine("  Content     : {0}", errorMessage);
                Console.WriteLine("  Reason      : {0}", response.ReasonPhrase);
                var resp = new HttpResponseMessage(response.StatusCode)
                {
                    Content = response.Content,
                    ReasonPhrase = response.ReasonPhrase
                };
                throw new System.Exception(resp.ToString());
            }

            
            return postResponse;
        }
    }
}
