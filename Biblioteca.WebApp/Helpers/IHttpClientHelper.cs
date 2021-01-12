using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Helpers
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> GetContent(string url);
        Task<HttpResponseMessage> PostContent(string url, HttpContent content);
    }
}
