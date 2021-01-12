using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {


        public async Task<HttpResponseMessage> GetContent(string url)
        {

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            HttpClient client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url);
            return response;

        }


        public async Task<HttpResponseMessage> PostContent(string url, HttpContent content)
        {

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            HttpClient client = new HttpClient(httpClientHandler);

            var response = await client.PostAsync(url, content);
            return response;

        }
    }
}
