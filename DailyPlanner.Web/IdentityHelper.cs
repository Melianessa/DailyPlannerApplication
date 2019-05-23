using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;


namespace DailyPlanner.Web
{
    public class IdentityHelper
    {
        private static HttpClient _client;

        public IdentityHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(Configuration["Url:Identity"])
            };
        }

        public IConfiguration Configuration { get; }

        public HttpClient InitializeClient(string token = null)
        {
            _client.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.SetBearerToken(token);
            }
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
        }
    }
    
}
