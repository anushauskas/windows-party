using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json.Serialization;

namespace DataAccesLayer
{
    public class DataService : IDataService, IDisposable
    {
        private HttpClient _httpClient;
        private string _token;

        public DataService()
        {
            _httpClient = new HttpClient();
        }


        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task Authenticate(LoginDetails loginDetails)
        {
            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var response = await _httpClient.PostAsync("http://playground.tesonet.lt/v1/tokens", loginDetails, formatter);
            var token = await response.Content.ReadAsAsync<TokenResponse>();
            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.Token}");
        }

        public async Task<IList<Server>> GetServerList()
        {
            var response = await _httpClient.GetAsync("http://playground.tesonet.lt/v1/servers");
            return await response.Content.ReadAsAsync<List<Server>>();
        }
    }
}
