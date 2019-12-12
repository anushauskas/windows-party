using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using DataAccesLayer;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Tests
{
    public class TestDataService
    {
        private IConfigurationRoot _configuration;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            
        }

        [Test]
        public async Task TestDataRetrieval()
        {
            LoginDetails loginDetails = new LoginDetails();
            _configuration.GetSection("LoginDetails").Bind(loginDetails);
            IList<Server> servers;
            using (DataService svc = new DataService())
            {
                await svc.Authenticate(loginDetails);
                servers = await svc.GetServerList();
            }
            Assert.IsNotEmpty(servers);
        }
    }
}