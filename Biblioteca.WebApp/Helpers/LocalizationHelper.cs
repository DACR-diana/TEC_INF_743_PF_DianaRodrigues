using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Helpers
{
    public class LocalizationHelper : ILocalizationHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public LocalizationHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

        }

        public async Task<object> GetContent(string lang, string controller, string url)
        {

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var result2 = "";

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{url}{lang}/api/{controller}/Get{controller}LanguageContent";

                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    result2 = JsonConvert.DeserializeObject<string>(result);
                }

            }
            return result2;

            //var request = new HttpRequestMessage(HttpMethod.Get, $"{url}{lang}/api/{controller}/Get{controller}LanguageContent");
            //var client = _clientFactory.CreateClient();
            //var response = await client.GetAsync($"{url}{lang}/api/{controller}/Get{controller}LanguageContent");

            //var content = response.Content.ReadAsStreamAsync();
            //var jsonContent = JsonConvert.SerializeObject(content);

            //return jsonContent;
           
        }


    }
}
