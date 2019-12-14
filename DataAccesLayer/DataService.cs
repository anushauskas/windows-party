using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json.Serialization;

namespace DataAccesLayer
{
    public class DataService : IDataService, IDisposable
    {
        private readonly HttpClient _httpClient;

        public DataService()
        {
            _httpClient = new HttpClient();
        }


        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<bool> Authenticate(LoginDetails loginDetails)
        {
            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var response = await _httpClient.PostAsync("http://playground.tesonet.lt/v1/tokens", loginDetails, formatter);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsAsync<TokenResponse>();
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.Token}");
                return true;
            }

            return false;
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Barer");
        }

        public async Task<IList<Server>> GetServerList()
        {
            var response = await _httpClient.GetAsync("http://playground.tesonet.lt/v1/servers");
            return await response.Content.ReadAsAsync<List<Server>>();
        }
    }
}
