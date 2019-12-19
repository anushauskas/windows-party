using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json.Serialization;
using Serilog;

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
                Log.Information("Login successful");
                var token = await response.Content.ReadAsAsync<TokenResponse>();
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.Token}");
                return true;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                Log.Warning($"Login failed: {message}");
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
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Server>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                Log.Error($"Data retrieval failed: {message}");
                throw new Exception("Server list retrieval failed");
            }
        }
    }
}
